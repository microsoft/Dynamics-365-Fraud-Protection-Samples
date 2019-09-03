// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;

namespace Microsoft.Dynamics.FraudProtection.Models
{
    public abstract class BaseFraudProtectionEvent
    {
#pragma warning disable IDE1006 // Naming Styles
        public EventMetadata _metadata { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }

    public class EventMetadata
    {
        public string TrackingId { get; set; }
        public DateTimeOffset MerchantTimeStamp { get; set; }
    }
}
