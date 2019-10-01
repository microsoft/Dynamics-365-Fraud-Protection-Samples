// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Interfaces;
using Contoso.FraudProtection.Infrastructure.Identity;
using Contoso.FraudProtection.Web.Extensions;
using Contoso.FraudProtection.Web.ViewModels;
using Contoso.FraudProtection.Web.ViewModels.Manage;
using Contoso.FraudProtection.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Dynamics.FraudProtection.Models;
using Microsoft.Dynamics.FraudProtection.Models.UpdateAccountEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.FraudProtection.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAppLogger<ManageController> _logger;
        private readonly IFraudProtectionService _fraudProtectionService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IAppLogger<ManageController> logger,
          IFraudProtectionService fraudProtectionService,
          IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _fraudProtectionService = fraudProtectionService;
            _contextAccessor = contextAccessor;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new IndexViewModel
            {
                Username = user.UserName,
                User = new UserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                },
                Address = new AddressViewModel
                {
                    Address1 = user.Address1,
                    Address2 = user.Address2,
                    City = user.City,
                    State = user.State,
                    ZipCode = user.ZipCode,
                    CountryRegion = user.CountryRegion,
                },
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = StatusMessage,
                DeviceFingerPrinting = new DeviceFingerPrintingViewModel
                {
                    SessionId = _contextAccessor.GetSessionId()
                }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Update the Application User
            user.FirstName = model.User.FirstName;
            user.LastName = model.User.LastName;
            user.Email = model.User.Email;
            user.UserName = model.Username;
            user.PhoneNumber = model.User.Phone;
            user.Address1 = model.Address.Address1;
            user.Address2 = model.Address.Address2;
            user.City = model.Address.City;
            user.State = model.Address.State;
            user.ZipCode = model.Address.ZipCode;
            user.CountryRegion = model.Address.CountryRegion;

            var result = await _userManager.UpdateAsync(user);

            #region Fraud Protection Service
            // If storing the user locally succeeds, update Fraud Protection
            if (result.Succeeded)
            {
                var billingAddress = new UserAddress
                {
                    Type = UserAddressType.Billing.ToString(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Street1 = user.Address1,
                    Street2 = user.Address2,
                    City = user.City,
                    State = user.State,
                    ZipCode = user.ZipCode,
                    Country = user.CountryRegion
                };

                var shippingAddress = new UserAddress
                {
                    Type = UserAddressType.Shipping.ToString(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Street1 = user.Address1,
                    Street2 = user.Address2,
                    City = user.City,
                    State = user.State,
                    ZipCode = user.ZipCode,
                    Country = user.CountryRegion
                };

                var fraudProtectionUser = new User
                {
                    UserId = user.Email,
                    UpdateDate = DateTimeOffset.Now,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    AddressList = new List<UserAddress> { billingAddress, shippingAddress },
                    ZipCode = user.ZipCode,
                    Country = user.CountryRegion,
                    TimeZone = new TimeSpan(0, 0, -model.DeviceFingerPrinting.ClientTimeZone, 0).ToString(),
                    DeviceContext = new DeviceContext
                    {
                        DeviceContextId = _contextAccessor.GetSessionId(),
                        IPAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DeviceContextDC = model.DeviceFingerPrinting.FingerPrintingDC,
                        Provider = DeviceContextProvider.DFPFingerPrinting.ToString()
                    }
                };

                var response = await _fraudProtectionService.PostUser(fraudProtectionUser);

                var fraudProtectionIO = new FraudProtectionIOModel(fraudProtectionUser, response, "UpdateAccount");
                TempData.Put(FraudProtectionIOModel.TempDataKey, fraudProtectionIO);
            }
            #endregion

            StatusMessage = "Your profile has been updated";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ManagePaymentInstrument()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var paymentInstrument = new ManagePaymentInstrumentViewModel
            {
                CreditCard = new CreditCardViewModel
                {
                    CardType = user.DefaultCardType,
                    CardNumber = user.DefaultCardNumber,
                    ExpirationMonth = user.DefaultExpirationMonth,
                    ExpirationYear = user.DefaultExpirationYear,
                    CardName = user.DefaultCardName,
                    CVV = user.DefaultCVV,
                },
                BillingAddress = new AddressViewModel
                {
                    Address1 = user.BillingAddress1,
                    Address2 = user.BillingAddress2,
                    City = user.BillingCity,
                    State = user.BillingState,
                    ZipCode = user.BillingZipCode,
                    CountryRegion = user.BillingCountryRegion,
                },
                StatusMessage = StatusMessage,
                DeviceFingerPrinting = new DeviceFingerPrintingViewModel
                {
                    SessionId = _contextAccessor.GetSessionId()
                }
            };

            return View(paymentInstrument);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagePaymentInstrument(ManagePaymentInstrumentViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Set card info
            user.DefaultCardType = model.CreditCard.CardType;
            user.DefaultCardNumber = model.CreditCard.CardNumber;
            user.DefaultCardName = model.CreditCard.CardName;
            user.DefaultCVV = model.CreditCard.CVV;
            user.DefaultExpirationMonth = model.CreditCard.ExpirationMonth;
            user.DefaultExpirationYear = model.CreditCard.ExpirationYear;

            // Set billing address info
            user.BillingAddress1 = model.BillingAddress.Address1;
            user.BillingAddress2 = model.BillingAddress.Address2;
            user.BillingCity = model.BillingAddress.City;
            user.BillingState = model.BillingAddress.State;
            user.BillingZipCode = model.BillingAddress.ZipCode;
            user.BillingCountryRegion = model.BillingAddress.CountryRegion;

            var updateResult = await _userManager.UpdateAsync(user);

            #region Fraud Protection Service
            // If storing the user's payment information succeeds, update Fraud Protection.
            if (updateResult.Succeeded)
            {
                var billingAddress = new AddressDetails
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Street1 = user.BillingAddress1,
                    Street2 = user.BillingAddress2,
                    City = user.BillingCity,
                    State = user.BillingState,
                    ZipCode = user.BillingZipCode,
                    Country = user.BillingCountryRegion
                };

                var userId = user.Email;

                var fraudProtectionUser = new User
                {
                    UserId = userId,
                    PaymentInstrumentList = new List<PaymentInstrument>
                    {
                        new PaymentInstrument
                        {
                            MerchantPaymentInstrumentId = $"{userId}-CreditCard",
                            Type = PaymentInstrumentType.CreditCard.ToString(),
                            CardType = model.CreditCard.CardType,
                            HolderName = model.CreditCard.CardName,
                            BIN = model.CreditCard.BIN,
                            ExpirationDate = model.CreditCard.ExpirationDate,
                            LastFourDigits = model.CreditCard.LastFourDigits,
                            BillingAddress = billingAddress,
                            CreationDate = DateTimeOffset.Now,
                            State = PaymentInstrumentState.Active.ToString(),
                        }
                    },
                    DeviceContext = new DeviceContext
                    {
                        DeviceContextId = _contextAccessor.GetSessionId(),
                        IPAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DeviceContextDC = model.DeviceFingerPrinting.FingerPrintingDC,
                        Provider = DeviceContextProvider.DFPFingerPrinting.ToString()
                    }
                };

                var response = await _fraudProtectionService.PostUser(fraudProtectionUser);

                var fraudProtectionIO = new FraudProtectionIOModel(fraudProtectionUser, response, "UpdateAccount");
                TempData.Put(FraudProtectionIOModel.TempDataKey, fraudProtectionIO);
            }
            #endregion

            StatusMessage = "Your payment information has been updated";
            return RedirectToAction(nameof(ManagePaymentInstrument));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToAction(nameof(SetPassword));
            }

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";

            return RedirectToAction(nameof(ChangePassword));
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return RedirectToAction(nameof(ChangePassword));
            }

            var model = new SetPasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                AddErrors(addPasswordResult);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "Your password has been set.";

            return RedirectToAction(nameof(SetPassword));
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLogins()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new ExternalLoginsViewModel { CurrentLogins = await _userManager.GetLoginsAsync(user) };
            model.OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => model.CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();
            model.ShowRemoveButton = await _userManager.HasPasswordAsync(user) || model.CurrentLogins.Count > 1;
            model.StatusMessage = StatusMessage;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkLogin(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action(nameof(LinkLoginCallback));
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> LinkLoginCallback()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync(user.Id);
            if (info == null)
            {
                throw new ApplicationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
            }

            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred adding external login for user with ID '{user.Id}'.");
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            StatusMessage = "The external login was added.";
            return RedirectToAction(nameof(ExternalLogins));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.RemoveLoginAsync(user, model.LoginProvider, model.ProviderKey);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred removing external login for user with ID '{user.Id}'.");
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "The external login was removed.";
            return RedirectToAction(nameof(ExternalLogins));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
