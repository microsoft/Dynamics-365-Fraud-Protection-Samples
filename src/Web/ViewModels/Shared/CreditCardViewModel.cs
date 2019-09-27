// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace Contoso.FraudProtection.Web.ViewModels.Shared
{
    public class CreditCardViewModel
    {
        [Required]
        [Display(Name = "Card Type")]
        public string CardType { get; set; }

        [Required]
        [Display(Name = "Card Number")]
        [RegularExpression("[0-9]{4}-?[0-9]{4}-?[0-9]{4}-?[0-9]{4}", ErrorMessage = "Card number must be 16 digits long. Dashes are optional.")]
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

        public string UnformattedCardNumber
        {
            get
            {
                return CardNumber?.Replace("-", "");
            }
        }

        #region Derived
        public string BIN => UnformattedCardNumber.Replace("-", "").Substring(0, 6);
        public string ExpirationDate => string.Join("/", ExpirationMonth, ExpirationYear);
        public string LastFourDigits => UnformattedCardNumber.Substring(UnformattedCardNumber.Length - 4);
        #endregion
    }
}
