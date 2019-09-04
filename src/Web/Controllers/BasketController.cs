// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.OrderAggregate;
using Contoso.FraudProtection.ApplicationCore.Interfaces;
using Contoso.FraudProtection.Infrastructure.Identity;
using Contoso.FraudProtection.Web.Extensions;
using Contoso.FraudProtection.Web.Interfaces;
using Contoso.FraudProtection.Web.Services;
using Contoso.FraudProtection.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Dynamics.FraudProtection.Models;
using Microsoft.Dynamics.FraudProtection.Models.BankEventEvent;
using Microsoft.Dynamics.FraudProtection.Models.PurchaseEvent;
using Microsoft.Dynamics.FraudProtection.Models.PurchaseStatusEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.FraudProtection.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class BasketController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBasketService _basketService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOrderService _orderService;
        private readonly IBasketViewModelService _basketViewModelService;
        private readonly IFraudProtectionService _fraudProtectionService;
        private readonly IHttpContextAccessor _contextAccessor;

        public BasketController(IBasketService basketService,
            IBasketViewModelService basketViewModelService,
            IOrderService orderService,
            SignInManager<ApplicationUser> signInManager,
            IFraudProtectionService fraudProtectionService,
            IHttpContextAccessor contextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _basketService = basketService;
            _signInManager = signInManager;
            _orderService = orderService;
            _basketViewModelService = basketViewModelService;
            _fraudProtectionService = fraudProtectionService;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var basketModel = await GetBasketViewModelAsync();

            return View(basketModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Dictionary<string, int> items)
        {
            var basketViewModel = await GetBasketViewModelAsync();
            await _basketService.SetQuantities(basketViewModel.Id, items);

            return View(await GetBasketViewModelAsync());
        }

        // POST: /Basket/AddToBasket
        [HttpPost]
        public async Task<IActionResult> AddToBasket(CatalogItemViewModel productDetails)
        {
            if (productDetails?.Id == null)
            {
                return RedirectToAction("Index", "Catalog");
            }
            var basketViewModel = await GetBasketViewModelAsync();

            await _basketService.AddItemToBasket(basketViewModel.Id, productDetails.Id, productDetails.Price, 1);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Dictionary<string, int> items)
        {
            var basketViewModel = await GetBasketViewModelAsync();
            await _basketService.SetQuantities(basketViewModel.Id, items);

            var user = await _userManager.GetUserAsync(User);

            var sessionId = _contextAccessor.GetSessionId();

            if (user == null)
            {
                //Anonymous user checkout.
                return View("CheckoutDetails", new CheckoutDetailsViewModel { NumberItems = basketViewModel.Items.Count, SessionId = sessionId });
            }

            // Apply default user settings
            var baseCheckoutModel = new CheckoutDetailsViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ShippingAddress1 = user.Address1,
                ShippingAddress2 = user.Address2,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode,
                CountryRegion = user.CountryRegion,
                CardType = user.DefaultCardType,
                CardName = user.DefaultCardName,
                CardNumber = user.DefaultCardNumber,
                ExpirationMonth = user.DefaultExpirationMonth,
                ExpirationYear = user.DefaultExpirationYear,
                CVV = user.DefaultCVV,
                BillingAddress1 = user.BillingAddress1,
                BillingAddress2 = user.BillingAddress2,
                BillingCity = user.BillingCity,
                BillingCountryRegion = user.BillingCountryRegion,
                BillingState = user.BillingState,
                BillingZipCode = user.BillingZipCode,
                NumberItems = basketViewModel.Items.Count,
                SessionId = sessionId
            };

            return View("CheckoutDetails", baseCheckoutModel);
        }

        public async Task<IActionResult> CheckoutDetails(CheckoutDetailsViewModel checkoutDetails)
        {
            var basketViewModel = await GetBasketViewModelAsync();

            var address = new Address(
                string.Join(' ', checkoutDetails.ShippingAddress1, checkoutDetails.ShippingAddress2),
                checkoutDetails.City,
                checkoutDetails.State,
                checkoutDetails.CountryRegion,
                checkoutDetails.ZipCode);

            var paymentInfo = new PaymentInfo(
                string.Join(' ', checkoutDetails.FirstName, checkoutDetails.LastName),
                checkoutDetails.CardNumber,
                checkoutDetails.CardType,
                checkoutDetails.CVV,
                string.Join('/', checkoutDetails.ExpirationMonth, checkoutDetails.ExpirationYear));

            #region Fraud Protection Service
            //Call Fraud Protection to get the risk score for this purchase.
            //We get a correlation ID that we pass to all Fraud Protection calls (up to 4) that we make in the purchase workflow.
            //This is optional, but can help when troubleshooting bugs or performance issues.
            var purchase = SetupPurchase(checkoutDetails, basketViewModel);
            var correlationId = _fraudProtectionService.NewCorrelationId;
            var result = await _fraudProtectionService.PostPurchase(purchase, correlationId);

            var fraudProtectionIO = new FraudProtectionIOModel(purchase, result, "Purchase");

            //Check the risk score that was returned and possibly complete the purchase.
            var status = await ApproveOrRejectPurchase(
                result.ResultDetails.MerchantRuleDecision,
                checkoutDetails.UnformattedCardNumber,
                purchase.PurchaseId,
                correlationId,
                fraudProtectionIO);

            TempData.Put(FraudProtectionIOModel.TempDataKey, fraudProtectionIO);
            #endregion

            await _orderService.CreateOrderAsync(basketViewModel.Id, address, paymentInfo, status, purchase, result);
            await _basketService.DeleteBasketAsync(basketViewModel.Id);

            ViewData["OrderResult"] = status;
            return View("Checkout");
        }

        #region Fraud Protection Service
        /// <summary>
        /// Checks the purchase's risk score.
        ///    If the purchase is not approved, submit a REJECTED purchase status.
        ///    If the purchase is approved, submit the bank AUTH, bank CHARGE, and purchase status (approved if the bank also approves the auth and charge, or rejected otherwise).
        /// </summary>
        private async Task<OrderStatus> ApproveOrRejectPurchase(string merchantRuleDecision, string cardNumber, string purchaseId, string correlationId, FraudProtectionIOModel fraudProtectionIO)
        {
            var status = OrderStatus.Received;
            BankEvent auth = null;
            BankEvent charge = null;
            PurchaseStatusEvent purchaseStatus;

            if (!FakeCreditCardBankResponses.CreditCardResponses.TryGetValue(cardNumber, out FakeCreditCardBankResponses creditCardBankResponse))
            {
                //default response
                creditCardBankResponse = new FakeCreditCardBankResponses
                {
                    IgnoreFraudRiskRecommendation = false,
                    IsAuthApproved = true,
                    IsChargeApproved = true
                };
            }

            if (!merchantRuleDecision.StartsWith("APPROVE", StringComparison.OrdinalIgnoreCase) &&
                !creditCardBankResponse.IgnoreFraudRiskRecommendation)
            {
                purchaseStatus = SetupPurchaseStatus(purchaseId, PurchaseStatusType.Rejected);
                status = OrderStatus.Rejected;
            }
            else
            {
                //Auth
                if (!creditCardBankResponse.IsAuthApproved)
                {
                    //Auth Rejected
                    auth = SetupBankEvent(BankEventType.AUTH, DateTimeOffset.Now, purchaseId, BankStatus.REJECTED);
                    //Purchase Status - Rejected
                    purchaseStatus = SetupPurchaseStatus(purchaseId, PurchaseStatusType.Rejected);
                    status = OrderStatus.Rejected;
                }
                else
                {
                    //Auth Approved
                    auth = SetupBankEvent(BankEventType.AUTH, DateTimeOffset.Now, purchaseId, BankStatus.APPROVED);
                    //Charge
                    if (creditCardBankResponse.IsChargeApproved)
                    {
                        //Charge - Approved
                        charge = SetupBankEvent(BankEventType.CHARGE, DateTimeOffset.Now, purchaseId, BankStatus.APPROVED);
                        //Purchase Status Approved
                        purchaseStatus = SetupPurchaseStatus(purchaseId, PurchaseStatusType.Approved);
                    }
                    else
                    {
                        //Charge - Rejected
                        charge = SetupBankEvent(BankEventType.CHARGE, DateTimeOffset.Now, purchaseId, BankStatus.REJECTED);
                        //Purchase status Rejected
                        purchaseStatus = SetupPurchaseStatus(purchaseId, PurchaseStatusType.Rejected);
                        status = OrderStatus.Rejected;
                    }
                }
            }

            if (auth != null)
            {
                var response = await _fraudProtectionService.PostBankEvent(auth, correlationId);
                fraudProtectionIO.Add(auth, response, "BankEvent Auth");
            }
            if (charge != null)
            {
                var response = await _fraudProtectionService.PostBankEvent(charge, correlationId);
                fraudProtectionIO.Add(charge, response, "BankEvent Charge");
            }
            if (purchaseStatus != null)
            {
                var response = await _fraudProtectionService.PostPurchaseStatus(purchaseStatus, correlationId);
                fraudProtectionIO.Add(purchaseStatus, response, "PurchaseStatus");
            }

            return status;
        }

        /// <summary>
        /// Creates the purchase event.
        /// </summary>
        private Purchase SetupPurchase(CheckoutDetailsViewModel checkoutDetails, BasketViewModel basketViewModel)
        {
            var shippingAddress = new AddressDetails
            {
                FirstName = checkoutDetails.FirstName,
                LastName = checkoutDetails.LastName,
                PhoneNumber = checkoutDetails.PhoneNumber,
                Street1 = checkoutDetails.ShippingAddress1,
                Street2 = checkoutDetails.ShippingAddress2,
                City = checkoutDetails.City,
                State = checkoutDetails.State,
                ZipCode = checkoutDetails.ZipCode,
                Country = checkoutDetails.CountryRegion
            };

            var billingAddress = new AddressDetails
            {
                FirstName = checkoutDetails.FirstName,
                LastName = checkoutDetails.LastName,
                PhoneNumber = checkoutDetails.PhoneNumber,
                Street1 = checkoutDetails.BillingAddress1,
                Street2 = checkoutDetails.BillingAddress2,
                City = checkoutDetails.BillingCity,
                State = checkoutDetails.BillingState,
                ZipCode = checkoutDetails.BillingZipCode,
                Country = checkoutDetails.BillingCountryRegion
            };

            var device = new DeviceContext
            {
                DeviceContextId = _contextAccessor.GetSessionId(),
                IPAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                DeviceContextDC = checkoutDetails.FingerPrintingDC,
                Provider = DeviceContextProvider.DFPFINGERPRINTING.ToString()
            };

            var productList = basketViewModel.Items
                .Select(i => new Product
                {
                    ProductId = i.Id.ToString(),
                    PurchasePrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    Margin = 2.1M,
                    IsPreorder = false,
                    ShippingMethod = PurchaseShippingMethod.Standard.ToString(),
                    ProductName = i.ProductName,
                    Type = "digital",
                    Category = i.CatalogItemId.ToString(),
                    Market = "US",
                    COGS = 0.11M,
                    IsRecurring = false,
                    SalesPrice = i.UnitPrice,
                    Currency = "USD",
                    IsFree = false,
                    Language = "EN-US",
                    Sku = i.Id.ToString()
                })
                .ToList();

            //Logged in vs. anonymous user checkout.
            //If they are logged in, use their email.
            //Otherwise, try to use the basket ID (from the cookie).
            //If that isn't available - it always should be - create a new GUID.
            var userId = User?.Identity?.Name ?? basketViewModel.BuyerId ?? Guid.NewGuid().ToString();

            var user = new UserDetails
            {
                UserId = userId,
                CreationDate = DateTimeOffset.Now,
                UpdateDate = DateTimeOffset.Now,
                FirstName = checkoutDetails.FirstName,
                LastName = checkoutDetails.LastName,
                Country = checkoutDetails.CountryRegion,
                ZipCode = checkoutDetails.ZipCode,
                TimeZone = TimeZoneInfo.Local.Id,
                Language = "EN-US",
                PhoneNumber = checkoutDetails.PhoneNumber,
                Email = checkoutDetails.Email,
                ProfileType = UserProfileType.Consumer.ToString()
            };

            var paymentInstrument = new PurchasePaymentInstrument
            {
                PurchaseAmount = basketViewModel.Total,
                MerchantPaymentInstrumentId = $"{userId}-CreditCard",
                Type = PaymentInstrumentType.CREDITCARD.ToString(),
                CardType = checkoutDetails.CardType,
                State = PaymentInstrumentState.Active.ToString(),
                HolderName = checkoutDetails.CardName,
                BIN = checkoutDetails.UnformattedCardNumber.Substring(0, 6),
                ExpirationDate = string.Join('/', checkoutDetails.ExpirationMonth, checkoutDetails.ExpirationYear),
                LastFourDigits = checkoutDetails.UnformattedCardNumber.Substring(checkoutDetails.UnformattedCardNumber.Length - 4),
                CreationDate = DateTimeOffset.Now.AddMonths(-14),
                BillingAddress = billingAddress,
            };

            return new Purchase
            {
                PurchaseId = Guid.NewGuid().ToString(),
                AssessmentType = AssessmentType.Protect.ToString(),
                ShippingAddress = shippingAddress,
                ShippingMethod = PurchaseShippingMethod.Standard.ToString(),
                Currency = "USD",
                DeviceContext = device,
                MerchantLocalDate = DateTimeOffset.Now,
                CustomerLocalDate = checkoutDetails.ClientDate,
                ProductList = productList,
                TotalAmount = basketViewModel.Total,
                SalesTax = basketViewModel.Tax,
                User = user,
                PaymentInstrumentList = new List<PurchasePaymentInstrument> { paymentInstrument },
            };
        }

        /// <summary>
        /// Creates purchase status events.
        /// </summary>
        private PurchaseStatusEvent SetupPurchaseStatus(string purchaseId, PurchaseStatusType status)
        {
            return new PurchaseStatusEvent
            {
                PurchaseId = purchaseId,
                StatusDate = DateTimeOffset.Now,
                StatusType = status.ToString(),
                Reason = "Some reason for " + status
            };
        }

        /// <summary>
        /// Creates charge and auth bank events.
        /// </summary>
        private BankEvent SetupBankEvent(BankEventType type, DateTimeOffset transactionDate, string PurchaseId, BankStatus status)
        {
            return new BankEvent
            {
                BankEventId = Guid.NewGuid().ToString(),
                Type = type.ToString(),
                BankEventTimestamp = transactionDate,
                Status = status.ToString(),
                BankResponseCode = "A000", //this is something the bank sent, here we just hardcode a possible one
                PaymentProcessor = "CitiAch", //this is something the bank sent, here we just hardcode a possible one
                MRN = "Z20LY1SSB3WY", //this is something the bank sent, here we just hardcode a possible one
                MID = "A1010EUSD01", //this is something the bank sent, here we just hardcode a possible one
                PurchaseId = PurchaseId
            };
        }
        #endregion

        private async Task<BasketViewModel> GetBasketViewModelAsync()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                return await _basketViewModelService.GetOrCreateBasketForUser(User.Identity.Name);
            }
            string anonymousId = GetOrSetBasketCookie();
            return await _basketViewModelService.GetOrCreateBasketForUser(anonymousId);
        }

        private string GetOrSetBasketCookie()
        {
            if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
            {
                return Request.Cookies[Constants.BASKET_COOKIENAME];
            }
            string anonymousId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions { Expires = DateTime.Today.AddYears(10) };
            Response.Cookies.Append(Constants.BASKET_COOKIENAME, anonymousId, cookieOptions);
            return anonymousId;
        }
    }
}
