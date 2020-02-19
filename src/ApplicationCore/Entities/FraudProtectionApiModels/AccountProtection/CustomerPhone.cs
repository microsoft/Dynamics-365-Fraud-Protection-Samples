// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class CustomerPhone
    {
        public string PhoneType { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsPhoneNumberValidated { get; set; }

        public DateTime PhoneNumberValidatedDate { get; set; }

        public bool IsPhoneUsername { get; set; } = false;
    }
}
