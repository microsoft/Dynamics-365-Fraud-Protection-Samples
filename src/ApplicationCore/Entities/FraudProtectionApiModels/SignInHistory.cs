using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels
{
    public class SignInHistory
    {
        public string SignUpIpAddress { get; set; }

        public string lastSignInIpAddress { get; set; }

        public string frequentSignInIpAddress { get; set; }

        public int failedSignInAttemptsInLastHour { get; set; }

        public int failedSignInAttemptsFromCurrentIpAddress { get; set; }
    }
}
