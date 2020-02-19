// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    /// <summary>
    /// Contains information and context about a signup attempt.
    /// </summary>
    public class SignUp : BaseFraudProtectionEvent
    {
        public User User { get; set; }

        public SSOAuthenticationProvider SSOAuthenticationProvider { get; set; }

        public CustomerEmail Email { get; set; }

        public CustomerPhone Phone { get; set; }

        public Address Address { get; set; }

        public PaymentInstrumentCard PaymentInstrumentCard { get; set; }

        public PaymentInstrumentPaypal PaymentInstrumentPaypal { get; set; }

        public DeviceContext Device { get; set; }

        public EventMetadataAccountCreate Metadata { get; set; }
    }

    public class EventMetadataAccountCreate : EventMetadata
    {
        public string SignUpId { get; set; }

        public DateTimeOffset CustomerLocalDate { get; set; }
    }
}
