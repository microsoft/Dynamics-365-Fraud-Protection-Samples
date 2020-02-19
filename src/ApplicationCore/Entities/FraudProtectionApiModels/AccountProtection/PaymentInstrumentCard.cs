// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class PaymentInstrumentCard : PaymentInstrument
    {
        public string CardType { get; set; }

        public string HolderName { get; set; }

        public string Bin { get; set; }

        public string ExpirationDate { get; set; }

        public string LastFourDigits { get; set; }

        public Address Address { get; set; }

        public PaymentInstrumentCard()
        {
            base.Name = "PaymentInstrument.Card";
        }
    }
}
