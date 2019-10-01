// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Dynamics.FraudProtection.Models.BankEventEvent
{
    /// <summary>
    /// Provides information about a purchase transaction that was sent to the bank as being approved or rejected when the transaction settles.
    /// </summary>
    public class BankEvent : BaseFraudProtectionEvent
    {
        /// <summary>
        /// A unique string identifying this Bank Event
        /// </summary>
        [Required]
        public String BankEventId { get; set; }

        /// <summary>
        /// See BankEventType enum.
        /// </summary>
        [Required]
        public String Type { get; set; }

        /// <summary>
        /// Timestamp from Bank
        /// </summary>
        public DateTimeOffset? BankEventTimestamp { get; set; }

        /// <summary>
        /// See BankStatus enum.
        /// </summary>
        public String Status { get; set; }

        /// <summary>
        /// Bank code on response
        /// </summary>
        public String BankResponseCode { get; set; }

        /// <summary>
        /// Processor name. Examples 'FDC', 'Adyen', 'TSYS', 'WorldPay', 'Chase', 'Stripe', 'PayPal'
        /// </summary>
        public String PaymentProcessor { get; set; }

        /// <summary>
        /// Merchant Reference Number, used to identify the transaction from the merchant side
        /// </summary>
        public String MRN { get; set; }

        /// <summary>
        /// The merchant ID (MID) sent to the bank.
        /// </summary>
        public String MID { get; set; }

        /// <summary>
        /// A unique string identifying the purchase
        /// </summary>
        [Required]
        public String PurchaseId { get; set; }
    }

    public enum BankEventType
    {
        Auth,
        Charge,
        AuthCancel,
        ChargeReversal
    }

    public enum BankEventStatus
    {
        Approved,
        Declined,
        Unknown
    }
}
