// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Dynamics.FraudProtection.Models;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    /// <summary>
    /// Contains information and context about a signup attempt.
    /// </summary>
    public class SignUp : BaseFraudProtectionEvent
    {
        public User User { get; set; }

        public SSOAuthenticationProvider SSOAuthenticationProvider { get; set; }

        public IList<CustomerEmail> Email { get; set; }

        public IList<CustomerPhone> Phone { get; set; }

        public IList<Address> Address { get; set; }

        public IList<PaymentInstrument> PaymentInstruments { get; set; }

        public DeviceContext Device { get; set; }

        public EventMetadataAccountCreate Metadata { get; set; }
    }

    public class EventMetadataAccountCreate : EventMetadata
    {
        public string SignUpId { get; set; }

        public DateTimeOffset CustomerLocalDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AssessmentType AssessmentType { get; set; }
    }
}
