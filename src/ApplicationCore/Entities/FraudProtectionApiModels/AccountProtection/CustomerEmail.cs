using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class CustomerEmail
    {
        public string EmailType { get; set; }

        public string EmailValue { get; set; }

        public bool IsEmailValidated { get; set; }

        public DateTime EmailValidatedDate { get; set; }

        public bool IsEmailUsername { get; set; } = false;
    }
}
