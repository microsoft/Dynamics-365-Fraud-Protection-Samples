// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;

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
