// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Text.Json.Serialization;

namespace Microsoft.Dynamics.FraudProtection.Models
{
    public abstract class BaseFraudProtectionEvent : IBaseFraudProtectionEvent
    {
        // Inlining to avoid multi-inheritance but still take advantage of shared UserDetails
        [JsonPropertyName("_metadata")]
        public EventMetadata Metadata { get; set; }
    }

    public class EventMetadata
    {
        public string TrackingId { get; set; }
        public DateTimeOffset MerchantTimeStamp { get; set; }
    }
}
