// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Interfaces;
using Contoso.FraudProtection.Infrastructure.Identity;
using Contoso.FraudProtection.Web.Extensions;
using Contoso.FraudProtection.Web.ViewModels;
using Contoso.FraudProtection.Web.ViewModels.Account;
using Contoso.FraudProtection.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Dynamics.FraudProtection.Models;
using Microsoft.Dynamics.FraudProtection.Models.SignupEvent;
using Microsoft.Dynamics.FraudProtection.Models.SignupStatusEvent;
using System;
using System.Threading.Tasks;

namespace Contoso.FraudProtection.Web.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IBasketService _basketService;
        private readonly IFraudProtectionService _fraudProtectionService;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IBasketService basketService,
            IFraudProtectionService fraudProtectionService,
            IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _basketService = basketService;
            _fraudProtectionService = fraudProtectionService;
            _contextAccessor = contextAccessor;
        }

        // GET: /Account/SignIn 
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            if (!String.IsNullOrEmpty(returnUrl) &&
                returnUrl.IndexOf("checkout", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                ViewData["ReturnUrl"] = "/Basket/Index";
            }

            return View();
        }

        // POST: /Account/SignIn
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ViewData["ReturnUrl"] = returnUrl;

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                await TransferBasketToEmailAsync(model.Email);
                return RedirectToLocal(returnUrl);
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(CatalogController.Index), "Catalog");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {
                DeviceFingerPrinting = new DeviceFingerPrintingViewModel
                {
                    SessionId = _contextAccessor.GetSessionId()
                }
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_contextAccessor.HttpContext.Connection == null)
                throw new Exception(nameof(_contextAccessor.HttpContext.Connection));

            //Create the user object and validate it before calling Fraud Protection
            var user = new ApplicationUser
            {
                UserName = model.User.Email,
                Email = model.User.Email,
                FirstName = model.User.FirstName,
                LastName = model.User.LastName,
                PhoneNumber = model.User.Phone,
                Address1 = model.Address.Address1,
                Address2 = model.Address.Address2,
                City = model.Address.City,
                State = model.Address.State,
                ZipCode = model.Address.ZipCode,
                CountryRegion = model.Address.CountryRegion
            };

            foreach (var v in _userManager.UserValidators)
            {
                var validationResult = await v.ValidateAsync(_userManager, user);
                if (!validationResult.Succeeded)
                {
                    AddErrors(validationResult);
                }
            };

            foreach (var v in _userManager.PasswordValidators)
            {
                var validationResult = await v.ValidateAsync(_userManager, user, model.Password);
                if (!validationResult.Succeeded)
                {
                    AddErrors(validationResult);
                }
            };

            if (ModelState.ErrorCount > 0)
            {
                return View(model);
            }

            #region Fraud Protection Service
            // Ask Fraud Protection to assess this signup/registration before registering the user in our database, etc.
            var signupAddress = new AddressDetails
            {
                FirstName = model.User.FirstName,
                LastName = model.User.LastName,
                PhoneNumber = model.User.Phone,
                Street1 = model.Address.Address1,
                Street2 = model.Address.Address2,
                City = model.Address.City,
                State = model.Address.State,
                ZipCode = model.Address.ZipCode,
                Country = model.Address.CountryRegion
            };

            var signupUser = new SignupUser
            {
                CreationDate = DateTimeOffset.Now,
                UpdateDate = DateTimeOffset.Now,
                FirstName = model.User.FirstName,
                LastName = model.User.LastName,
                Country = model.Address.CountryRegion,
                ZipCode = model.Address.ZipCode,
                TimeZone = new TimeSpan(0, 0, -model.DeviceFingerPrinting.ClientTimeZone, 0).ToString(),
                Language = "EN-US",
                PhoneNumber = model.User.Phone,
                Email = model.User.Email,
                ProfileType = UserProfileType.Consumer.ToString(),
                Address = signupAddress
            };

            var deviceContext = new DeviceContext
            {
                DeviceContextId = _contextAccessor.GetSessionId(),
                IPAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                DeviceContextDC = model.DeviceFingerPrinting.FingerPrintingDC,
                Provider = DeviceContextProvider.DFPFingerPrinting.ToString(),
            };

            var marketingContext = new MarketingContext
            {
                Type = MarketingType.Direct.ToString(),
                IncentiveType = MarketingIncentiveType.None.ToString(),
                IncentiveOffer = "Integrate with Fraud Protection"
            };

            var storefrontContext = new StoreFrontContext
            {
                StoreName = "Fraud Protection Sample Site",
                Type = StorefrontType.Web.ToString(),
                Market = "US"
            };

            var signupEvent = new SignUp
            {
                SignUpId = Guid.NewGuid().ToString(),
                AssessmentType = AssessmentType.Protect.ToString(),
                User = signupUser,
                MerchantLocalDate = DateTimeOffset.Now,
                CustomerLocalDate = model.DeviceFingerPrinting.ClientDate,
                MarketingContext = marketingContext,
                StoreFrontContext = storefrontContext,
                DeviceContext = deviceContext,
            };

            var correlationId = _fraudProtectionService.NewCorrelationId;

            var signupAssessment = await _fraudProtectionService.PostSignup(signupEvent, correlationId);

            //Track Fraud Protection request/response for display only
            var fraudProtectionIO = new FraudProtectionIOModel(signupEvent, signupAssessment, "Signup");

            //2 out of 3 signups will succeed on average. Adjust if you want more or less signups blocked for tesing purposes.
            var rejectSignup = new Random().Next(0, 3) != 0;
            var signupStatusType = rejectSignup ? SignupStatusType.Rejected.ToString() : SignupStatusType.Approved.ToString();

            var signupStatus = new SignupStatusEvent
            {
                SignUpId = signupEvent.SignUpId,
                StatusType = signupStatusType,
                StatusDate = DateTimeOffset.Now,
                Reason = "User is " + signupStatusType
            };

            if (!rejectSignup)
            {
                signupStatus.User = new SignupStatusUser { UserId = model.User.Email };
            }

            var signupStatusResponse = await _fraudProtectionService.PostSignupStatus(signupStatus, correlationId);

            fraudProtectionIO.Add(signupStatus, signupStatusResponse, "Signup Status");

            TempData.Put(FraudProtectionIOModel.TempDataKey, fraudProtectionIO);

            if (rejectSignup)
            {
                ModelState.AddModelError("", "Signup rejected by Fraud Protection. You can try again as it has a random likelyhood of happening in this sample site.");
                return View(model);
            }
            #endregion

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            await TransferBasketToEmailAsync(user.Email);

            return RedirectToLocal(returnUrl);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(CatalogController.Index), "Catalog");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied(string returnUrl)
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(CatalogController.Index), "Catalog");
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        private async Task TransferBasketToEmailAsync(string email)
        {
            string anonymousBasketId = Request.Cookies[Constants.BASKET_COOKIENAME];
            if (!string.IsNullOrEmpty(anonymousBasketId))
            {
                await _basketService.TransferBasketAsync(anonymousBasketId, email);
                Response.Cookies.Delete(Constants.BASKET_COOKIENAME);
            }
        }
    }
}
