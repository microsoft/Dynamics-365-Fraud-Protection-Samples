// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace Contoso.FraudProtection.Web.ViewModels.Manage
{
    public class ManagePaymentInstrumentViewModel
    {
        [Required]
        [Display(Name = "Card Type")]
        public string CardType { get; set; }

        [Required]
        [Display(Name = "Card Number")]
        [RegularExpression("[0-9]{4}(-[0-9]{4}){2}-[0-9]{4}", ErrorMessage = "Card number must be 16 digits long and separated with -")]
        public string CardNumber { get; set; }

        [Required]
        [Display(Name = "Expiration Month")]
        public string ExpirationMonth { get; set; }

        [Required]
        [Display(Name = "Expiration Year")]
        public string ExpirationYear { get; set; }

        [Required]
        [RegularExpression("([0-9]{3})|([0-9]{4})", ErrorMessage = "CVV must be 3 or 4 digits long")]
        public string CVV { get; set; }

        [Required]
        [Display(Name = "Card Holder Name")]
        public string CardName { get; set; }

        [Required]
        [Display(Name = "Address Line 1")]
        public string Address1 { get; set; }
        
        [Display(Name = "Address Line 2")]
        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "Country/Region")]
        public string CountryRegion { get; set; }

        public string StatusMessage { get; set; }

        public string FingerPrintingDC { get; set; }
    }
}
