// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Collections.Generic;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class ResponseSuccess : Response
    {
        public List<ResultDetail> ResultDetails { get; set; }
    }
}
