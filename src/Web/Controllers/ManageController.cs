// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Interfaces;
using Contoso.FraudProtection.Infrastructure.Identity;
using Contoso.FraudProtection.Web.Extensions;
using Contoso.FraudProtection.Web.ViewModels;
using Contoso.FraudProtection.Web.ViewModels.Manage;
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
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Contoso.FraudProtection.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IAppLogger<ManageController> _logger;
        private readonly UrlEncoder _urlEncoder;
        private readonly IFraudProtectionService _fraudProtectionService;
        private readonly IHttpContextAccessor _contextAccessor;

        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender,
          IAppLogger<ManageController> logger,
          UrlEncoder urlEncoder,
          IFraudProtectionService fraudProtectionService,
          IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _urlEncoder = urlEncoder;
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
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address1 = user.Address1,
                Address2 = user.Address2,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode,
                CountryRegion = user.CountryRegion,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = StatusMessage
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
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Username;
            user.PhoneNumber = model.PhoneNumber;
            user.Address1 = model.Address1;
            user.Address2 = model.Address2;
            user.City = model.City;
            user.State = model.State;
            user.ZipCode = model.ZipCode;
            user.CountryRegion = model.CountryRegion;

            var result = await _userManager.UpdateAsync(user);

            #region Fraud Protection Service
            // If storing the user locally succeeds, update Fraud Protection
            if (result.Succeeded)
            {
                var billingAddress = new UserAddress
                {
                    Type = UserAddressType.BILLING.ToString(),
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
                    Type = UserAddressType.SHIPPING.ToString(),
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
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    AddressList = new List<UserAddress> { billingAddress, shippingAddress },
                    ZipCode = user.ZipCode,
                    Country = user.CountryRegion,
                    TimeZone = new TimeSpan(0, 0, -model.ClientTimeZone, 0).ToString(),
                    DeviceContext = new DeviceContext
                    {
                        DeviceContextId = _contextAccessor.GetSessionId(),
                        IPAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DeviceContextDC = model.FingerPrintingDC,
                        Provider = DeviceContextProvider.DFPFINGERPRINTING.ToString()
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
                CardType = user.DefaultCardType,
                CardNumber = user.DefaultCardNumber,
                ExpirationMonth = user.DefaultExpirationMonth,
                ExpirationYear = user.DefaultExpirationYear,
                CardName = user.DefaultCardName,
                CVV = user.DefaultCVV,
                Address1 = user.BillingAddress1,
                Address2 = user.BillingAddress2,
                City = user.BillingCity,
                State = user.BillingState,
                ZipCode = user.BillingZipCode,
                CountryRegion = user.BillingCountryRegion,
                StatusMessage = StatusMessage
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
            user.DefaultCardType = model.CardType;
            user.DefaultCardNumber = model.CardNumber;
            user.DefaultCardName = model.CardName;
            user.DefaultCVV = model.CVV;
            user.DefaultExpirationMonth = model.ExpirationMonth;
            user.DefaultExpirationYear = model.ExpirationYear;

            // Set billing address info
            user.BillingAddress1 = model.Address1;
            user.BillingAddress2 = model.Address2;
            user.BillingCity = model.City;
            user.BillingState = model.State;
            user.BillingZipCode = model.ZipCode;
            user.BillingCountryRegion = model.CountryRegion;

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
                            Type = PaymentInstrumentType.CREDITCARD.ToString(),
                            CardType = user.DefaultCardType,
                            HolderName = model.CardName,
                            BIN = user.BIN,
                            ExpirationDate = user.ExpirationDate,
                            LastFourDigits = user.LastFourDigits,
                            BillingAddress = billingAddress,
                            CreationDate = DateTimeOffset.Now,
                            State = PaymentInstrumentState.Active.ToString(),
                        }
                    },
                    DeviceContext = new DeviceContext
                    {
                        DeviceContextId = _contextAccessor.GetSessionId(),
                        IPAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                        DeviceContextDC = model.FingerPrintingDC,
                        Provider = DeviceContextProvider.DFPFINGERPRINTING.ToString()
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
