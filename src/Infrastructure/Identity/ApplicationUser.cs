// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Identity;

namespace Contoso.FraudProtection.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string CountryRegion { get; set; }

        public string DefaultCardType { get; set; }

        public string DefaultCardName { get; set; }

        public string DefaultCardNumber { get; set; }

        public string DefaultExpirationMonth { get; set; }

        public string DefaultExpirationYear { get; set; }

        public string DefaultCVV { get; set; }

        public string BillingAddress1 { get; set; }

        public string BillingAddress2 { get; set; }

        public string BillingCity{ get; set; }

        public string BillingState { get; set; }

        public string BillingZipCode { get; set; }

        public string BillingCountryRegion { get; set; }
    }
}
