// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System;

namespace Contoso.FraudProtection.ApplicationCore.Entities.BuyerAggregate
{
    public class Buyer : BaseEntity, IAggregateRoot
    {
        public string IdentityGuid { get; private set; }

        private List<PaymentMethod> _paymentMethods = new List<PaymentMethod>();

        public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

        private Buyer()
        {
            // required by EF
        }

        public Buyer(string identity) : this()
        {
            if (string.IsNullOrEmpty(identity))
                throw new ArgumentNullException(identity);

            IdentityGuid = identity;
        }
    }
}
