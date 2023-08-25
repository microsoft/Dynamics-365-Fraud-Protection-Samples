// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response;
using System.Collections.Generic;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection.Response
{
    public class Response
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public List<ResultDetail> ResultDetails { get; set; }

        public string TransactionReferenceId { get; set; }

        public Enrichments<AccountProtectionDeviceAttributes> Enrichments { get; set; }

        public IDictionary<string, IEnumerable<object>> Diagnostics { get; set; }
    }
}
