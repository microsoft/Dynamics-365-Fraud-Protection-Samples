// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection.Response
{
    public class AccountProtectionDeviceAttributes
    {
        public string BrowserUserAgent { get; set; }
        public string BrowserUserAgentLanguages { get; set; }
        public string Carrier { get; set; }
        public string DataNetworkType { get; set; }
        public string DeviceAsn { get; set; }
        public string DeviceCity { get; set; }
        public string DeviceCountryCode { get; set; }
        public string DeviceId { get; set; }
        public string DeviceModelName { get; set; }
        public string DevicePostalCode { get; set; }
        public string DeviceState { get; set; }
        public string DeviceSystemVersion { get; set; }
        public string IpRoutingType { get; set; }
        public string IsDeviceEmulator { get; set; }
        public string IsDeviceRooted { get; set; }
        public string IsWifiEnabled { get; set; }
        public string LocalIpv4 { get; set; }
        public string LocalIpv6 { get; set; }
        public string Platform { get; set; }
        public string Proxy { get; set; }
        public string ProxyIp { get; set; }
        public string ProxyLastDetected { get; set; }
        public string ProxyType { get; set; }
        public string RealtimeTimezoneOffset { get; set; }
        public string SdkType { get; set; }
        public string Sld { get; set; }
        public string TcpDistance { get; set; }
        public string TimeZone { get; set; }
        public string TrueIp { get; set; }
        public string UserAgentBrowser { get; set; }
        public string UserAgentOperatingSystem { get; set; }
        public string UserAgentPlatform { get; set; }

    }
}
