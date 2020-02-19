// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Dynamics.FraudProtection.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

        public DateTimeOffset CustomerLocalDate { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AssessmentType AssessmentType { get; set; }
    }

    public class RecentUpdate
    {
        public DateTimeOffset LastPhoneNumberUpdateDate { get; set; }

        public DateTimeOffset LastEmailUpdateDate { get; set; }

        public DateTimeOffset LastAddressUpdateDate { get; set; }

        public DateTimeOffset LastPaymentInstrumentUpdateDate { get; set; }
    }
}