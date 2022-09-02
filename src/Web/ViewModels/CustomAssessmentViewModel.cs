﻿// Copyright (c) Microsoft Corporation.
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

        [Required]
        public EndpointVersion Version { get; set; }
    }

    public enum EndpointVersion
    {
        V1,
        V2,
        V2Observe,
        V2Label
    }
}