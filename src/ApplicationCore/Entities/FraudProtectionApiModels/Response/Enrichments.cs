// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response
{
    public class Enrichments
    {
        public DeviceAttributes DeviceAttributes { get; set; }

        public CalculatedFeatures CalculatedFeatures { get; set; }
    }

    public class DeviceAttributes
    {
        public string TrueIp { get; set; }

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

        public string ProxyIp { get; set; }

        public string RealtimeTimezoneOffset { get; set; }

        public string TimeZone { get; set; }

        public string Sld { get; set; }

        public string ProxyLastDetected { get; set; }

        public string ProxyType { get; set; }
    }

    public class CalculatedFeatures
    {
        public string EmailDomain { get; set; }

        public string EmailPattern { get; set; }
    }
}
