// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AccountProtection = Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection;

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
            var model = new LoginViewModel
            {
                DeviceFingerPrinting = new DeviceFingerPrintingViewModel
                {
                    SessionId = _contextAccessor.GetSessionId()
                }
            };
            ViewData["ReturnUrl"] = returnUrl;
            if (!String.IsNullOrEmpty(returnUrl) &&
                returnUrl.IndexOf("checkout", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                ViewData["ReturnUrl"] = "/Basket/Index";
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignInAP(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View("SignIn", model);
            }
            ViewData["ReturnUrl"] = returnUrl;

            return await SignInUser(model, returnUrl, true);
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

            return await SignInUser(model, returnUrl, false);
        }

        private async Task<IActionResult> SignInUser(LoginViewModel model, string returnUrl, bool useAP)
        {
            var applicationUser = new ApplicationUser
            {
                UserName = model.Email
            };
            string hashedPassword = _userManager.PasswordHasher.HashPassword(applicationUser, model.Password);

            bool rejectSignIn = false;

            if (useAP)
            {
                var user = new AccountProtection.User()
                {
                    UserType = AccountProtection.UserType.Consumer,
                    Username = model.Email,
                    UserId = model.Email
                };

                var device = new AccountProtection.DeviceContext()
                {
                    DeviceContextId = model.DeviceFingerPrinting.SessionId,
                    IpAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                    Provider = DeviceContextProvider.DFPFingerPrinting.ToString()
                };

                var metadata = new AccountProtection.EventMetadataAccountLogin()
                {
                    TrackingId = Guid.NewGuid().ToString(),
                    LoginId = Guid.NewGuid().ToString(),
                    CustomerLocalDate = DateTime.Now,
                    MerchantTimeStamp = DateTime.Now
                };

                var signIn = new AccountProtection.SignIn()
                {
                    Name = "AP.AccountLogin",
                    Version = "0.5",
                    Device = device,
                    User = user,
                    Metadata = metadata
                };

                var correlationId = _fraudProtectionService.NewCorrelationId;
                var signInResponse = await _fraudProtectionService.PostSignInAP(signIn, correlationId);

                var fraudProtectionIO = new FraudProtectionIOModel(correlationId, signIn, signInResponse, "SignIn");
                TempData.Put(FraudProtectionIOModel.TempDataKey, fraudProtectionIO);

                if (signInResponse is AccountProtection.ResponseSuccess response)
                {
                    rejectSignIn = response.ResultDetails.FirstOrDefault()?.Decision != AccountProtection.DecisionName.Approve;
                }
            }
            else
            {
                var signIn = new SignIn
                {
                    SignInId = Guid.NewGuid().ToString(),
                    PasswordHash = hashedPassword,
                    MerchantLocalDate = DateTimeOffset.Now,
                    CustomerLocalDate = model.DeviceFingerPrinting.ClientDate,
                    UserId = model.Email,
                    DeviceContextId = model.DeviceFingerPrinting.SessionId,
                    AssessmentType = AssessmentType.Protect.ToString(),
                    CurrentIpAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()
                };

                var correlationId = _fraudProtectionService.NewCorrelationId;
                var signInResponse = await _fraudProtectionService.PostSignIn(signIn, correlationId);

                var fraudProtectionIO = new FraudProtectionIOModel(correlationId, signIn, signInResponse, "SignIn");
                TempData.Put(FraudProtectionIOModel.TempDataKey, fraudProtectionIO);

                //2 out of 3 signIn will be successful
                rejectSignIn = new Random().Next(0, 3) != 0;
            }

            if (!rejectSignIn)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View("SignIn", model);
                }
                // redirect if signIn is not rejected and password sign-in is success
                await TransferBasketToEmailAsync(model.Email);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Signin rejected by Fraud Protection. You can try again as it has a random likelihood of happening in this sample site.");
                return View("SignIn", model);
            }
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
        public async Task<IActionResult> RegisterAP(RegisterViewModel model, string returnUrl = null)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }

            if (_contextAccessor.HttpContext.Connection == null)
            {
                throw new Exception(nameof(_contextAccessor.HttpContext.Connection));
            }

            return await RegisterUser(model, returnUrl, true);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_contextAccessor.HttpContext.Connection == null)
            {
                throw new Exception(nameof(_contextAccessor.HttpContext.Connection));
            }

            return await RegisterUser(model, returnUrl, false);
        }

        private async Task<IActionResult> RegisterUser(RegisterViewModel model, string returnUrl, bool useAP)
        {
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
                return View("Register", model);
            }

            #region Fraud Protection Service
            var correlationId = _fraudProtectionService.NewCorrelationId;

            // Ask Fraud Protection to assess this signup/registration before registering the user in our database, etc.
            if (useAP)
            {
                AccountProtection.SignUp signupEvent = CreateSignupAPEvent(model);

                var signupAssessment = await _fraudProtectionService.PostSignupAP(signupEvent, correlationId);

                //Track Fraud Protection request/response for display only
                var fraudProtectionIO = new FraudProtectionIOModel(correlationId, signupEvent, signupAssessment, "Signup");
                TempData.Put(FraudProtectionIOModel.TempDataKey, fraudProtectionIO);

                bool rejectSignup = false;
                if (signupAssessment is AccountProtection.ResponseSuccess signupResponse)
                {
                    rejectSignup = signupResponse.ResultDetails.FirstOrDefault()?.Decision != AccountProtection.DecisionName.Approve;
                }

                if (rejectSignup)
                {
                    ModelState.AddModelError("", "Signup rejected by Fraud Protection. You can try again as it has a random likelihood of happening in this sample site.");
                    return View("Register", model);
                }
            }
            else
            {
                SignUp signupEvent = CreateSignupEvent(model);

                var signupAssessment = await _fraudProtectionService.PostSignup(signupEvent, correlationId);

                //Track Fraud Protection request/response for display only
                var fraudProtectionIO = new FraudProtectionIOModel(correlationId, signupEvent, signupAssessment, "Signup");

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
                    ModelState.AddModelError("", "Signup rejected by Fraud Protection. You can try again as it has a random likelihood of happening in this sample site.");
                    return View("Register", model);
                }
            }
            #endregion

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                AddErrors(result);
                return View("Register", model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            await TransferBasketToEmailAsync(user.Email);

            return RedirectToLocal(returnUrl);
        }

        private AccountProtection.SignUp CreateSignupAPEvent(RegisterViewModel model)
        {
            var signupUser = new AccountProtection.User()
            {
                Username = model.User.Email,
                FirstName = model.User.FirstName,
                LastName = model.User.LastName,
                CountryRegion = model.Address.CountryRegion,
                ZipCode = model.Address.ZipCode,
                TimeZone = new TimeSpan(0, 0, -model.DeviceFingerPrinting.ClientTimeZone, 0).ToString(),
                Language = "EN-US",
                UserType = AccountProtection.UserType.Consumer,
            };

            var customerEmail = new AccountProtection.CustomerEmail()
            {
                EmailType = AccountProtection.EmailType.Primary,
                EmailValue = model.User.Email,
                IsEmailValidated = false,
                IsEmailUsername = true
            };

            var customerPhone = new AccountProtection.CustomerPhone()
            {
                PhoneType = AccountProtection.PhoneType.Primary,
                PhoneNumber = model.User.Phone,
                IsPhoneNumberValidated = false,
                IsPhoneUsername = false
            };

            var address = new AccountProtection.Address()
            {
                AddressType = AccountProtection.AddressType.Primary,
                FirstName = model.User.FirstName,
                LastName = model.User.LastName,
                PhoneNumber = model.User.Phone,
                Street1 = model.Address.Address1,
                Street2 = model.Address.Address2,
                City = model.Address.City,
                State = model.Address.State,
                ZipCode = model.Address.ZipCode,
                CountryRegion = model.Address.CountryRegion
            };

            var device = new AccountProtection.DeviceContext()
            {
                DeviceContextId = model.DeviceFingerPrinting.SessionId,
                IpAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
                Provider = DeviceContextProvider.DFPFingerPrinting.ToString()
            };

            var metadata = new AccountProtection.EventMetadataAccountCreate()
            {
                TrackingId = Guid.NewGuid().ToString(),
                SignUpId = Guid.NewGuid().ToString(),
                CustomerLocalDate = DateTime.Now,
                MerchantTimeStamp = DateTime.Now,
                AssessmentType = AssessmentType.Evaluate
            };

            AccountProtection.SignUp signupEvent = new AccountProtection.SignUp()
            {
                Name = "AP.AccountCreation",
                Version = "0.5",
                User = signupUser,
                Email = new List<AccountProtection.CustomerEmail>() { customerEmail },
                Phone = new List<AccountProtection.CustomerPhone>() { customerPhone },
                Address = new List<AccountProtection.Address>() { address },
                Device = device,
                Metadata = metadata
            };
            return signupEvent;
        }

        private SignUp CreateSignupEvent(RegisterViewModel model)
        {
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
            return signupEvent;
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
