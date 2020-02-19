// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class CustomerPhone
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PhoneType PhoneType { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsPhoneNumberValidated { get; set; }

        public DateTimeOffset PhoneNumberValidatedDate { get; set; }

        public bool IsPhoneUsername { get; set; } = false;
    }

    public enum PhoneType
    {
        Primary,
        Alternative
    }
}
