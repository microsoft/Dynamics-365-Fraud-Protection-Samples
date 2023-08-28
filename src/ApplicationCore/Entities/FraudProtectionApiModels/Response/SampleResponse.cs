// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response
{
    public class SampleResponseRaw
    {
        public object RawData { get; set; }
    }

    public class SampleResponse<T> : SampleResponseRaw
    {
        public T Data { get; set; }
    }
}
