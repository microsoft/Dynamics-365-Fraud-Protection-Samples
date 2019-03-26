// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Contoso.FraudProtection.ApplicationCore.Entities.OrderAggregate
{
    public class Address // ValueObject
    {
        public string Street { get; private set; }

        public string City { get; private set; }

        public string State { get; private set; }

        public string CountryRegion { get; private set; }

        public string ZipCode { get; private set; }

        private Address() { }

        public Address(string street, string city, string state, string countryRegion, string zipcode)
        {
            Street = street;
            City = city;
            State = state;
            CountryRegion = countryRegion;
            ZipCode = zipcode;
        }
    }
}
