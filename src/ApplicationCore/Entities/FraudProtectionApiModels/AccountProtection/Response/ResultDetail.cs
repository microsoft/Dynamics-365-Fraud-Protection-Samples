// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Text.Json.Serialization;
using System.Collections.Generic;
using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection.Response
{
    public class ResultDetail
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DecisionName Decision { get; set; }

        public string ChallengeType { get; set; }

        public string[] Reasons { get; set; }

        public string Rule { get; set; }

        public string ClauseName { get; set; }

        public string[] SupportMessages { get; set; }

        public Dictionary<string, object> Other { get; set; }

        public List<Score> Scores { get; set; }

        public IEnumerable<RuleEvaluation> RuleEvaluations { get; set; }
    }

    public enum DecisionName
    {
        Approve,
        Reject,
        Challenge,
        Review
    }
}
