// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Contoso.FraudProtection.ApplicationCore.Entities.OrderAggregate
{
    public class PaymentInfo
    {
        public string CardholderName { get; private set; }

        public string Last4DigitsCardNumber { get; private set; }
        
        public string CardType { get; private set; }

        public string CVV { get; private set; }

        public string ExpirationDate { get; private set; }

        private PaymentInfo() { }

        public PaymentInfo(string cardholderName, string cardNumber, string cardType, string cvv, string expDate)
        {
            CardholderName = cardholderName;
            Last4DigitsCardNumber = cardNumber.Substring(cardNumber.Length - 4);
            CardType = cardType;
            CVV = cvv;
            ExpirationDate = expDate;
        }
    }
}
