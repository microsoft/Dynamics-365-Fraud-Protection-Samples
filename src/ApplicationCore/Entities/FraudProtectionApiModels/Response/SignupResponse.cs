// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.


using System.Text.Json.Serialization;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels
{
    public class SignupResponse
    {
        [JsonPropertyName("resultDetails")]
        public SignupResponseDetails ResultDetails { get; set; }
    }

    public class SignupResponseDetails : BaseAssessmentResponse
    {
        public string SignUpId { get; set; }
    }
}
