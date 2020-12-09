// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Collections.Generic;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection.Response
{
    public class ResponseError : Response
    {
        public bool IsRetryable { get; set; }

        public string Message { get; set; }

        public List<ResponseError> OtherErrors { get; set; }
    }
}
