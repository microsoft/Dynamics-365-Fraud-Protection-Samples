// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class EventMetadata
    {
        public string Name { get; set; }

        public string TrackingId { get; set; }

        public DateTime MerchantTimeStamp { get; set; }
    }
}
