using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.FraudProtection.Web.ViewModels
{
    public class CustomAssessmentViewModel
    {
        [Required]  
        public string ApiName { get; set; }

        [Required]
        public string Payload { get; set; }
    }
}