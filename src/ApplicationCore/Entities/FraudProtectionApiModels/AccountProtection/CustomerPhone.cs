// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Text.Json.Serialization;
using System;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class CustomerPhone
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PhoneType PhoneType { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsPhoneNumberValidated { get; set; }

        public DateTimeOffset? PhoneNumberValidatedDate { get; set; }

        public bool IsPhoneUsername { get; set; } = false;
    }

    public enum PhoneType
    {
        Primary,
        Alternative
    }
}
