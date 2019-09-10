// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Dynamics.FraudProtection.Models
{
    public interface IBaseFraudProtectionEvent
    {
        EventMetadata Metadata { get; set; }
    }
}