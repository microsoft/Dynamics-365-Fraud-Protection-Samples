// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response;
using System.Collections.Generic;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection.Response
{
    public class ResponseSuccess : Response
    {
        public List<ResultDetail> ResultDetails { get; set; }

        public string TransactionReferenceId { get; set; }

        public Enrichments<AccountProtectionDeviceAttributes> Enrichments { get; set; }

        public IDictionary<string, IEnumerable<object>> Diagnostics { get; set; }
    }
}
