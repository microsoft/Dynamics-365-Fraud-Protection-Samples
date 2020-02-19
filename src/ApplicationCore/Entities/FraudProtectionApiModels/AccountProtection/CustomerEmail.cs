// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class CustomerEmail
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public EmailType EmailType { get; set; }

        public string EmailValue { get; set; }

        public bool IsEmailValidated { get; set; }

        public DateTimeOffset? EmailValidatedDate { get; set; }

        public bool IsEmailUsername { get; set; } = false;
    }

    public enum EmailType
    {
        Primary,
        Alternative
    }
}
