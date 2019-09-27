// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;

namespace Contoso.FraudProtection.Web.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public int ClientTimeZone { get; set; }

        public DateTime ClientDate { get; set; }

        public string ClientCountryCode { get; set; }

        public DeviceFingerPrintingModel DeviceFingerPrinting { get; set; }
    }
}
