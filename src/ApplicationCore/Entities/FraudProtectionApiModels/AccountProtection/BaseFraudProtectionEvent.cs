// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public abstract class BaseFraudProtectionEvent
    {
        public string Name { get; set; }

        public string Version { get; set; }
    }
}
