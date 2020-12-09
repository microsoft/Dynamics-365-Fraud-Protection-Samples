// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection.Response
{
    public class AccountProtectionDeviceAttributes : DeviceAttributes
    {
        public string ProxyIp { get; set; }

        public string RealtimeTimezoneOffset { get; set; }

        public string TimeZone { get; set; }

        public string Sld { get; set; }

        public string ProxyLastDetected { get; set; }

        public string ProxyType { get; set; }
    }
}
