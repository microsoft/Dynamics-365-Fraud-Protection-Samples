// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Net;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels
{
    public class BaseResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
    }
}
