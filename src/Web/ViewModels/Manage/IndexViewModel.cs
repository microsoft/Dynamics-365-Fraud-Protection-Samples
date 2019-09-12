// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;

namespace Contoso.FraudProtection.Web.ViewModels.Manage
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone(ErrorMessage = "Phone should include country code and area code. e.g. +1-1234567890")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        [Display(Name = "Country/Region")]
        public string CountryRegion { get; set; }

        public string StatusMessage { get; set; }

        public int ClientTimeZone { get; set; }

        public DateTime ClientDate { get; set; }

        public string ClientCountryCode { get; set; }

        public DeviceFingerPrintingModel DeviceFingerPrinting { get; set; }
    }
}
