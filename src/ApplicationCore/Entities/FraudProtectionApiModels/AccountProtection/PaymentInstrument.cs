// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Dynamics.FraudProtection.Models;
using System;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class PaymentInstrument
    {
        public string MerchantPaymentInstrumentId { get; set; }

        public PaymentInstrumentType Type { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
