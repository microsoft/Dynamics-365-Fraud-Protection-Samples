// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.Web.ViewModels.Shared;

namespace Contoso.FraudProtection.Web.ViewModels.Manage
{
    public class ManagePaymentInstrumentViewModel
    {
        public AddressViewModel BillingAddress { get; set; }

        public DeviceFingerPrintingViewModel DeviceFingerPrinting { get; set; }

        public CreditCardViewModel CreditCard { get; set; }

        public string StatusMessage { get; set; }
    }
}
