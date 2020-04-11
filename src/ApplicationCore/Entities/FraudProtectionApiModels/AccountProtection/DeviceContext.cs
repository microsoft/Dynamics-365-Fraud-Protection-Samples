// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class DeviceContext
    {
        public string DeviceContextId { get; set; }

        public string IpAddress { get; set; }

        public string Provider { get; set; }

        public string ExternalDeviceId { get; set; }

        public string ExternalDeviceType { get; set; }
    }
}
