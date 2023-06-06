using System;
using System.Collections.Generic;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response
{
    public class AssessmentResponse
    {
        public AssessmentMetadata Metadata { get; set; }

        public AssessmentDecisionDetails DecisionDetails { get; set; }

        public List<RuleEvaluation> RuleEvaluations { get; set; }

        public AssessmentEnrichments Enrichments { get; set; }

        public IDictionary<string, object> CustomProperties { get; set; }

        public IDictionary<string, object> Diagnostics { get; set; }
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

    public class AssessmentEnrichments
    {
        public AssessmentDeviceAttributes DeviceAttributes { get; set; }

        public AssessmentScores Risk { get; set; }

        public AssessmentScores Bot { get; set; }
    }

    public class AssessmentDeviceAttributes
    {
        public string TrueIp { get; set; }

        public string RealtimeTimezoneOffset { get; set; }

        public string Sld { get; set; }
        
        public string TimeZone { get; set; }
        
        public string DeviceId { get; set; }
        
        public string DeviceCountryCode { get; set; }
        
        public string DeviceState { get; set; }
        
        public string DeviceCity { get; set; }
        
        public string DevicePostalCode { get; set; }
        
        public string DeviceAsn { get; set; }
        
        public string Platform { get; set; }
        
        public string BrowserUserAgentLanguages { get; set; }
        
        public string BrowserUserAgent { get; set; }
        
        public string TcpDistance { get; set; }
        
        public string Carrier { get; set; }
        
        public string IpRoutingType { get; set; }
        
        public string Proxy { get; set; }
        
        public string UserAgentPlatform { get; set; }
        
        public string UserAgentBrowser { get; set; }
        
        public string UserAgentOperatingSystem { get; set; }
        
        public string SdkType { get; set; }
        
        public string UserAgentType { get; set; }
        
        public string CookieEnabled { get; set; }
        
        public string FontsCount { get; set; }
        
        public string JavaScriptEnabled { get; set; }
        
        public string MimeTypesCount { get; set; }
        
        public string PluginsCount { get; set; }
        
        public string ScreenResolution { get; set; }
        
        public string TimeZoneOffset { get; set; }
    }

    public class AssessmentScores
    {
        public string ScoreType { get; set; }
        
        public int ScoreValue { get; set; }
        
        public string Reason { get; set; }
    }

}