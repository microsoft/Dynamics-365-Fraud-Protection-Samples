using System.Text.Json.Serialization;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response
{
    public class SignInResponse
    {
        [JsonPropertyName("resultDetails")]
        public SignInResponseDetails ResultDetails { get; set; }
    }

    public class SignInResponseDetails : BaseAssessmentResponse
    {
        public string SignInId { get; set; }
    }

}
