using Microsoft.Dynamics.FraudProtection.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels
{
    public class SignInRequest : BaseFraudProtectionEvent
    {

        public string SignInId { get; set; }

        public string PasswordHash { get; set; }

        public string CurrentIpAddress { get; set; }

        public string AssessmentType { get; set; }

        public DateTimeOffset CustomerLocalDate { get; set; }

        public DateTimeOffset MerchantLocalDate { get; set; }

        public string UserId { get; set; }

        public string DeviceContextId { get; set; }

        public SignInHistory SignInHistory { get; set; }

    }
}
