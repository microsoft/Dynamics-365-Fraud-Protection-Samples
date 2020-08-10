// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection.Responses
{
    public class Enrichment
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
    }

    public class CalculatedFeatures
    {
        public string EmailDomain { get; set; }

        public string EmailPattern { get; set; }
    }
}
