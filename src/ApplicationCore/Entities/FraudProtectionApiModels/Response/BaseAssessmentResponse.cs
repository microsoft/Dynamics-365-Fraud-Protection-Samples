// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels
{
    public abstract class BaseAssessmentResponse
    {
        public string MerchantRuleDecision { get; set; }
        public int RiskScore { get; set; }
        public string ReasonCodes { get; set; }
    }
}
