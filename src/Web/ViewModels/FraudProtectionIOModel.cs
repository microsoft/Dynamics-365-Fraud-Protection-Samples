// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Newtonsoft.Json;

namespace Contoso.FraudProtection.Web.ViewModels
{
    public class FraudProtectionIOModel
    {
        public const string TempDataKey = "FraudProtectionIOData";

        public readonly string FraudProtectionRequest;
        public readonly string FraudProtectionResponse;

        public FraudProtectionIOModel(object request, object response)
        {
            FraudProtectionRequest = JsonConvert.SerializeObject(request, Formatting.Indented);
            FraudProtectionResponse = JsonConvert.SerializeObject(response, Formatting.Indented);
        }
    }
}