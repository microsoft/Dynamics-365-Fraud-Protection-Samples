// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public abstract class Response
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}
