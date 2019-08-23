// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Newtonsoft.Json;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels
{
    public class PurchaseResponse: BaseResponse
    {
        [JsonProperty("resultDetails")]
        public PurchaseResultDetails ResultDetails { get; set; }
    }

    public class PurchaseResultDetails : BaseAssessmentResponse
    {
        public string MIDFlag { get; set; }
        public string PolicyApplied { get; set; }
    }
}
