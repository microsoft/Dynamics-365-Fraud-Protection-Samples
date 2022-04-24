// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.OrderAggregate;
using Contoso.FraudProtection.ApplicationCore.Interfaces;
using Contoso.FraudProtection.Infrastructure.Identity;
using Contoso.FraudProtection.Web.Extensions;
using Contoso.FraudProtection.Web.Interfaces;
using Contoso.FraudProtection.Web.Services;
using Contoso.FraudProtection.Web.ViewModels;
using Contoso.FraudProtection.Web.ViewModels.Shared;
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
                return View("CheckoutDetails", new CheckoutDetailsViewModel
                {
                    NumberItems = basketViewModel.Items.Count,
                    DeviceFingerPrinting = new DeviceFingerPrintingViewModel
                    {
                        SessionId = sessionId
                    }
                });
            }

            // Apply default user settings
            var baseCheckoutModel = new CheckoutDetailsViewModel
            {
                User = new UserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                },
                ShippingAddress = new AddressViewModel
                {
                    Address1 = user.Address1,
                    Address2 = user.Address2,
                    City = user.City,
                    State = user.State,
                    ZipCode = user.ZipCode,
                    CountryRegion = user.CountryRegion,
                },
                CreditCard = new CreditCardViewModel
                {
                    CardType = user.DefaultCardType,
                    CardName = user.DefaultCardName,
                    CardNumber = user.DefaultCardNumber,
                    ExpirationMonth = user.DefaultExpirationMonth,
                    ExpirationYear = user.DefaultExpirationYear,
                    CVV = user.DefaultCVV,
                },
                BillingAddress = new AddressViewModel
                {
                    Address1 = user.BillingAddress1,
                    Address2 = user.BillingAddress2,
                    City = user.BillingCity,
                    CountryRegion = user.BillingCountryRegion,
                    State = user.BillingState,
                    ZipCode = user.BillingZipCode,
                },
                NumberItems = basketViewModel.Items.Count,
                DeviceFingerPrinting = new DeviceFingerPrintingViewModel
                {
                    SessionId = sessionId
                }
            };

            return View("CheckoutDetails", baseCheckoutModel);
        }

        public async Task<IActionResult> CheckoutDetails(CheckoutDetailsViewModel checkoutDetails)
        {
            var basketViewModel = await GetBasketViewModelAsync();

            var address = new Address(
                string.Join(' ', checkoutDetails.ShippingAddress.Address1, checkoutDetails.ShippingAddress.Address2),
                checkoutDetails.ShippingAddress.City,
                checkoutDetails.ShippingAddress.State,
                checkoutDetails.ShippingAddress.CountryRegion,
                checkoutDetails.ShippingAddress.ZipCode);

            var paymentInfo = new PaymentInfo(
                string.Join(' ', checkoutDetails.User.FirstName, checkoutDetails.User.LastName),
                checkoutDetails.CreditCard.CardNumber,
                checkoutDetails.CreditCard.CardType,
                checkoutDetails.CreditCard.CVV,
                string.Join('/', checkoutDetails.CreditCard.ExpirationMonth, checkoutDetails.CreditCard.ExpirationYear));

            #region Fraud Protection Service
            //Call Fraud Protection to get the risk score for this purchase.
            //We get a correlation ID that we pass to all Fraud Protection calls (up to 4) that we make in the purchase workflow.
            //This is optional, but can help when troubleshooting bugs or performance issues.
            var purchase = SetupPurchase(checkoutDetails, basketViewModel);
            var correlationId = _fraudProtectionService.NewCorrelationId;
            var result = await _fraudProtectionService.PostPurchase(purchase, correlationId, HttpContext.Session.GetString("envId"));

            var fraudProtectionIO = new FraudProtectionIOModel(correlationId, purchase, result, "Purchase");

            //Check the risk score that was returned and possibly complete the purchase.
            var status = await ApproveOrRejectPurchase(
                result.ResultDetails.MerchantRuleDecision,
                checkoutDetails.CreditCard.UnformattedCardNumber,
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
        ///    If the purchase is rejected, submit a REJECTED purchase status.
        ///    If the purchase is approved, submit the bank AUTH, bank CHARGE, and purchase status (approved if the bank also approves the auth and charge, or rejected otherwise).
        ///    If the purchase is in review, submit unknown bank AUTH and unknown purchase status (so that case management case is still created)
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

            var isApproved = merchantRuleDecision.StartsWith("APPROVE", StringComparison.OrdinalIgnoreCase);
            var inReview = merchantRuleDecision.StartsWith("REVIEW", StringComparison.OrdinalIgnoreCase);
            if (isApproved || inReview || creditCardBankResponse.IgnoreFraudRiskRecommendation)
            {
                if (!creditCardBankResponse.IsAuthApproved)
                {
                    //Auth - Rejected
                    auth = SetupBankEvent(BankEventType.Auth, DateTimeOffset.Now, purchaseId, BankEventStatus.Declined);
                    //Purchase status - Rejected
                    purchaseStatus = SetupPurchaseStatus(purchaseId, PurchaseStatusType.Rejected);
                    status = OrderStatus.Rejected;
                }
                else
                {
                    //Auth - Approved/Unknown
                    var authStatus = inReview ? BankEventStatus.Unknown : BankEventStatus.Approved;
                    auth = SetupBankEvent(BankEventType.Auth, DateTimeOffset.Now, purchaseId, authStatus);

                    //Charge
                    var chargeStatus = creditCardBankResponse.IsChargeApproved ? BankEventStatus.Approved : BankEventStatus.Declined;
                    if (!inReview)
                    {
                        charge = SetupBankEvent(BankEventType.Charge, DateTimeOffset.Now, purchaseId, chargeStatus);
                    }

                    if (inReview)
                    {
                        //Purchase status - Unknown
                        purchaseStatus = SetupPurchaseStatus(purchaseId, PurchaseStatusType.Unknown);
                        status = OrderStatus.InReview;
                    }
                    else if (creditCardBankResponse.IsChargeApproved)
                    {
                        //Purchase status - Approved
                        purchaseStatus = SetupPurchaseStatus(purchaseId, PurchaseStatusType.Approved);
                    }
                    else
                    {
                        //Purchase status - Rejected
                        purchaseStatus = SetupPurchaseStatus(purchaseId, PurchaseStatusType.Rejected);
                        status = OrderStatus.Rejected;
                    }
                }
            }
            else
            {
                purchaseStatus = SetupPurchaseStatus(purchaseId, PurchaseStatusType.Rejected);
                status = OrderStatus.Rejected;
            }

            if (auth != null)
            {
                var response = await _fraudProtectionService.PostBankEvent(auth, correlationId, HttpContext.Session.GetString("envId"));
                fraudProtectionIO.Add(auth, response, "BankEvent Auth");
            }
            if (charge != null)
            {
                var response = await _fraudProtectionService.PostBankEvent(charge, correlationId, HttpContext.Session.GetString("envId"));
                fraudProtectionIO.Add(charge, response, "BankEvent Charge");
            }
            if (purchaseStatus != null)
            {
                var response = await _fraudProtectionService.PostPurchaseStatus(purchaseStatus, correlationId, HttpContext.Session.GetString("envId"));
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
                FirstName = checkoutDetails.User.FirstName,
                LastName = checkoutDetails.User.LastName,
                PhoneNumber = checkoutDetails.User.Phone,
                Street1 = checkoutDetails.ShippingAddress.Address1,
                Street2 = checkoutDetails.ShippingAddress.Address2,
                City = checkoutDetails.ShippingAddress.City,
                State = checkoutDetails.ShippingAddress.State,
                ZipCode = checkoutDetails.ShippingAddress.ZipCode,
                Country = checkoutDetails.ShippingAddress.CountryRegion
            };

            var billingAddress = new AddressDetails
            {
                FirstName = checkoutDetails.User.FirstName,
                LastName = checkoutDetails.User.LastName,
                PhoneNumber = checkoutDetails.User.Phone,
                Street1 = checkoutDetails.BillingAddress.Address1,
                Street2 = checkoutDetails.BillingAddress.Address2,
                City = checkoutDetails.BillingAddress.City,
                State = checkoutDetails.BillingAddress.State,
                ZipCode = checkoutDetails.BillingAddress.ZipCode,
                Country = checkoutDetails.BillingAddress.CountryRegion
            };

            var device = new DeviceContext
            {
                DeviceContextId = _contextAccessor.GetSessionId(),
                ExternalDeviceId = Guid.NewGuid().ToString(),
                IPAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                Provider = DeviceContextProvider.DFPFingerPrinting.ToString()
            };

            static string getCategoryFromName(string productName)
            {
                if (productName.Contains("mug", StringComparison.InvariantCultureIgnoreCase))
                {
                    return ProductCategory.HomeGarden.ToString();
                }

                if (productName.Contains("shirt", StringComparison.InvariantCultureIgnoreCase))
                {
                    return ProductCategory.ClothingShoes.ToString();
                }

                if (productName.Contains("sheet", StringComparison.InvariantCultureIgnoreCase))
                {
                    return ProductCategory.Jewelry.ToString();
                }

                return "Other";
            }

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
                    Type = ProductType.Digital.ToString(),
                    Category = getCategoryFromName(i.ProductName),
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
                FirstName = checkoutDetails.User.FirstName,
                LastName = checkoutDetails.User.LastName,
                Country = checkoutDetails.ShippingAddress.CountryRegion,
                ZipCode = checkoutDetails.ShippingAddress.ZipCode,
                TimeZone = TimeZoneInfo.Local.Id,
                Language = "EN-US",
                PhoneNumber = checkoutDetails.User.Phone,
                Email = checkoutDetails.User.Email,
                ProfileType = UserProfileType.Consumer.ToString()
            };

            var paymentInstrument = new PurchasePaymentInstrument
            {
                PurchaseAmount = basketViewModel.Total,
                MerchantPaymentInstrumentId = $"{userId}-CreditCard",
                Type = PaymentInstrumentType.CreditCard.ToString(),
                CardType = checkoutDetails.CreditCard.CardType,
                State = PaymentInstrumentState.Active.ToString(),
                HolderName = checkoutDetails.CreditCard.CardName,
                BIN = checkoutDetails.CreditCard.UnformattedCardNumber.Substring(0, 6),
                ExpirationDate = string.Join('/', checkoutDetails.CreditCard.ExpirationMonth, checkoutDetails.CreditCard.ExpirationYear),
                LastFourDigits = checkoutDetails.CreditCard.UnformattedCardNumber.Substring(checkoutDetails.CreditCard.UnformattedCardNumber.Length - 4),
                CreationDate = DateTimeOffset.Now.AddMonths(-14),
                BillingAddress = billingAddress,
            };

            Product mostExpensiveProduct = productList.FirstOrDefault();
            foreach (var product in productList)
            {
                if ((product.SalesPrice / product.Quantity) > (mostExpensiveProduct.SalesPrice / mostExpensiveProduct.Quantity))
                {
                    mostExpensiveProduct = product;
                }
            }

            var customData = new Dictionary<string, object>()
            {
                { "MostExpensiveProduct", mostExpensiveProduct.ProductName },
                { "TotalCOGS", productList.Sum(p => p.COGS * p.Quantity) },
                { "ContainsRiskyProducts", productList.Any(p => p.Category == ProductCategory.Jewelry.ToString() || p.Category == ProductCategory.HomeGarden.ToString()) }
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
                CustomerLocalDate = checkoutDetails.DeviceFingerPrinting.ClientDate,
                ProductList = productList,
                TotalAmount = basketViewModel.Total,
                SalesTax = basketViewModel.Tax,
                User = user,
                PaymentInstrumentList = new List<PurchasePaymentInstrument> { paymentInstrument },
                CustomData = customData
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
                Reason = PurchaseStatusReason.RuleEngine.ToString()
            };
        }

        /// <summary>
        /// Creates charge and auth bank events.
        /// </summary>
        private BankEvent SetupBankEvent(BankEventType type, DateTimeOffset transactionDate, string PurchaseId, BankEventStatus status)
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
