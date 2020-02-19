// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;

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
