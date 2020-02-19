// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Newtonsoft.Json;
using System;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public abstract class BaseFraudProtectionEvent
    {
        public string Name { get; set; }

        public string Version { get; set; }
    }
}
