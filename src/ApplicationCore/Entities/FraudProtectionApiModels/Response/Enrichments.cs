// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response
{
    public class Enrichments<TDeviceAttributes> where TDeviceAttributes : DeviceAttributes
    {
        public TDeviceAttributes DeviceAttributes { get; set; }

        public CalculatedFeatures CalculatedFeatures { get; set; }
    }

    public class DeviceAttributes
    {
        public string BrowserUserAgent { get; set; }

        public string BrowserUserAgentLanguages { get; set; }

        public string Carrier { get; set; }

        public string CookieEnabled { get; set; }

        public string DeviceAsn { get; set; }

        public string DeviceCity { get; set; }

        public string DeviceCountryCode { get; set; }

        public string DeviceId { get; set; }

        public string DevicePostalCode { get; set; }

        public string DeviceState { get; set; }

        public string FontsCount { get; set; }

        public string IpRoutingType { get; set; }

        public string JavaScriptEnabled { get; set; }

        public string MimeTypesCount { get; set; }

        public string Platform { get; set; }

        public string Plugins { get; set; }

        public string PluginsCount { get; set; }

        public string Proxy { get; set; }

        public string ScreenResolution { get; set; }

        public string TcpDistance { get; set; }

        public string TimeZoneOffset { get; set; }

        public string TrueIp { get; set; }

        public string UserAgentBrowser { get; set; }

        public string UserAgentOperatingSystem { get; set; }

        public string UserAgentPlatform { get; set; }

        public string UserAgentType { get; set; }
    }

    public class CalculatedFeatures
    {
        public string EmailDomain { get; set; }

        public string EmailPattern { get; set; }
    }
}
