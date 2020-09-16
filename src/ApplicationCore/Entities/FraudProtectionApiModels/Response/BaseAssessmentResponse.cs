// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Collections.Generic;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels
{
    public abstract class BaseAssessmentResponse
    {
        public string MerchantRuleDecision { get; set; }
        public int RiskScore { get; set; }
        public string ReasonCodes { get; set; }
        public IEnumerable<string> RuleEvaluations { get; set; }
        public string ClauseName { get; set; }
        public DeviceAttributes DeviceAttributes { get; set; }
    }
}
