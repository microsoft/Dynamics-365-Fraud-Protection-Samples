using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class DeviceContext
    {
        public string sessionID { get; set; }

        public string IpAddress { get; set; }

        public string Provider { get; set; }

        public string ExternalDeviceId { get; set; }

        public string ExternalDeviceType { get; set; }

    }
}
