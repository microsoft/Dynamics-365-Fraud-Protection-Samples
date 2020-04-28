// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Text.Json.Serialization;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class User
    {
        public string UserId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserType UserType { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CountryRegion { get; set; }

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
