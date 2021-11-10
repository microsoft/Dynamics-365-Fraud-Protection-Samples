// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.Web.ViewModels.Shared;

namespace Contoso.FraudProtection.Web.ViewModels
{
    public class CheckoutDetailsViewModel
    {
        public UserViewModel User { get; set; }

        public AddressViewModel ShippingAddress { get; set; }

        public AddressViewModel BillingAddress { get; set; }

        public DeviceFingerPrintingViewModel DeviceFingerPrinting { get; set; }

        public CreditCardViewModel CreditCard { get; set; }

        public int NumberItems { get; set; }

        public string EnvironmentId { get; set; }
    }
}
