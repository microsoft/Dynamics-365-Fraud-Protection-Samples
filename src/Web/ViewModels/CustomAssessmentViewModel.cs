// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;

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