using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class Address
    {
        public string AddressType { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Street1 { get; set; }

        public string Street2 { get; set; }

        public string Street3 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string District { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }
    }
}
