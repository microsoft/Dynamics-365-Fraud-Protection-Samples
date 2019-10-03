using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response
{
    public class SignInResponse
    {
        [JsonProperty("resultDetails")]
        public SignInResponseDetails ResultDetails { get; set; }
    }

    public class SignInResponseDetails : BaseAssessmentResponse
    {
        public string SignInId { get; set; }
    }

}
