// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Newtonsoft.Json;
using System;

namespace Microsoft.Dynamics.FraudProtection.Models
{
    public abstract class BaseFraudProtectionEvent : IBaseFraudProtectionEvent
    {
        // Inlining to avoid multi-inheritance but still take advantage of shared UserDetails
        [JsonProperty(PropertyName = "_metadata")]
        public EventMetadata Metadata { get; set; }
    }

    public class EventMetadata
    {
        public string TrackingId { get; set; }
        public DateTimeOffset MerchantTimeStamp { get; set; }
    }
}
