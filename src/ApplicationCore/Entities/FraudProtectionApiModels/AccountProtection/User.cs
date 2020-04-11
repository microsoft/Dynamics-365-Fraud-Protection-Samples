// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class User
    {
        public string UserId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public UserType UserType { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public string TimeZone { get; set; }

        public string Language { get; set; }

        public string MembershipId { get; set; }
    }

    public enum UserType
    {
        Consumer,
        Developer,
        Seller,
        Publisher,
        Tenant
    }
}
