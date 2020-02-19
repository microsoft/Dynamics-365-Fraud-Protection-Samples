// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Dynamics.FraudProtection.Models;
using System;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    /// <summary>
    /// Contains information and context about a sign-in attempt.
    /// </summary>
    public class SignIn: BaseFraudProtectionEvent
    {
        public DeviceContext Device { get; set; }

        public User User { get; set; }

        public SSOAuthenticationProvider SSOAuthenticationProvider { get; set; }

        public EventMetadataAccountLogin Metadata { get; set; }

        public RecentUpdate RecentUpdate { get; set; }
    }

    public class EventMetadataAccountLogin : EventMetadata
    {
        public string LoginId { get; set; }

        public DateTime CustomerLocalDate { get; set; }

        public AssessmentType AssessmentType { get; set; }
    }

    public class RecentUpdate
    {
        public DateTime LastPhoneNumberUpdateDate { get; set; }

        public DateTime LastEmailUpdateDate { get; set; }

        public DateTime LastAddressUpdateDate { get; set; }

        public DateTime LastPaymentInstrumentUpdateDate { get; set; }
    }
}