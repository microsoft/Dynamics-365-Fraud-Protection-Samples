// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;

namespace Contoso.FraudProtection.Web.ViewModels
{
    public class CheckoutDetailsViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName{ get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Phone(ErrorMessage = "Phone should include country code and area code. e.g. +1-1234567890")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Shipping Address 1")]
        public string ShippingAddress1 { get; set; }

        [Required]
        [Display(Name = "Shipping Address 2")]
        public string ShippingAddress2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        [Display(Name = "Country/Region")]
        public string CountryRegion { get; set; }

        [Required]
        [Display(Name = "Card Type")]
        public string CardType { get; set; }

        [Required]
        [Display(Name = "Card Number")]
        [RegularExpression("[0-9]{4}(-[0-9]{4}){2}-[0-9]{4}", ErrorMessage = "Card number must be 16 digits long and separated with -")]
        public string CardNumber { get; set; }

        public string UnformattedCardNumber {
            get
            {
                return CardNumber?.Replace("-", "");
            }
        }

        [Required]
        [Display(Name = "Expiration Month")]
        public string ExpirationMonth { get; set; }

        [Required]
        [Display(Name = "Expiration Year")]
        public string ExpirationYear { get; set; }

        [Required]
        public string CVV { get; set; }

        [Required]
        [Display(Name = "Card Holder Name")]
        public string CardName { get; set; }

        [Required]
        [Display(Name = "Billing Address Line 1")]
        public string BillingAddress1 { get; set; }

        [Display(Name = "Billing Address Line 2")]
        public string BillingAddress2 { get; set; }

        [Required]
        public string BillingCity { get; set; }

        [Required]
        public string BillingState { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public string BillingZipCode { get; set; }

        [Required]
        [Display(Name = "Billing Country/Region")]
        public string BillingCountryRegion { get; set; }

        public int ClientTimeZone { get; set; }

        public DateTimeOffset ClientDate { get; set; }

        public string ClientCountryCode { get; set; }

        public string FingerPrintingDC { get; set; }

        public int NumberItems { get; set; }

        public string SessionId { get; set; }
    }
}
