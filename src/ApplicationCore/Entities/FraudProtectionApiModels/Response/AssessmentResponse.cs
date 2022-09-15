using System;
using System.Collections.Generic;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response
{
    public class AssessmentResponse
    {
        public AssessmentMetadata Metadata { get; set; }

        public AssessmentDecisionDetails DecisionDetails { get; set; }

        public List<RuleEvaluation> RuleEvaluations { get; set; }

        public List<AssessmentScores> Scores { get; set; }

        public IDictionary<string, object> CustomProperties { get; set; }
    }

    public class AssessmentMetadata
    {
        public string EventId { get; set; }

        public string SessionId { get; set; }

        public string GdprId { get; set; }

        public bool IsTest { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public DateTimeOffset ProcessedDateTime { get; set; }

        public string HierarchyPath { get; set; }

        public string EventType { get; set; }

        public string CorrelationId { get; set; }
    }

    public class AssessmentDecisionDetails
    {
        public string MerchantRuleDecision { get; set; }

        public string ChallengeType { get; set; }

        public string Rule { get; set; }

        public string ClauseName { get; set; }

        public List<string> Reasons { get; set; }

        public List<string> SupportMessages { get; set; }
    }

    public class AssessmentScores
    {
        public string ScoreType { get; set; }

        public double ScoreValue { get; set; }

        public string Reason { get; set; }
    }
}
