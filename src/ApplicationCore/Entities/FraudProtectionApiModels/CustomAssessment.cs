using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels
{
    public class CustomAssessment
    {
        [Required]
        public string ApiName { get; set; }

        [Required]
        public string Payload { get; set; }
    }
}
