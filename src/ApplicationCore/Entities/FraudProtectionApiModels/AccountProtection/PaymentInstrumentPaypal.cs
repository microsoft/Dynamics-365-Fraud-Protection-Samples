using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class PaymentInstrumentPaypal : PaymentInstrument
    {
        public string Email { get; set; }

        public string BillingAgreementId { get; set; }

        public string PayerId { get; set; }

        public string PayerStatus { get; set; }

        public string AddressStatus { get; set; }

        public PaymentInstrumentPaypal()
        {
            base.Name = "PaymentInstrument.Paypal";
        }
    }
}
