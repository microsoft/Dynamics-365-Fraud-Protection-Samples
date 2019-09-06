// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Newtonsoft.Json;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels
{
    public class SignupResponse
    {
        [JsonProperty("resultDetails")]
        public SignupResponseDetails ResultDetails { get; set; }
    }

    public class SignupResponseDetails : BaseAssessmentResponse
    {
        public string SignUpId { get; set; }
    }
}
