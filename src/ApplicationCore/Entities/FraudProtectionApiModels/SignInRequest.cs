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

        // enum?
        public string AssessmentType { get; set; }

        public DateTime CustomerLocalDate { get; set; }
        public DateTime MerchantLocalDate { get; set; }
        public string UserId { get; set; }
        public string DeviceContextId { get; set; }
        //public string signInId { get; set; }

    }
}
