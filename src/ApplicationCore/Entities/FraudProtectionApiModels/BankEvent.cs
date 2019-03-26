// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Dynamics.FraudProtection.Models.BankEventEvent
{
    /// <summary>
    /// 	Provides information about a purchase transaction that was sent to the bank as being approved or rejected when the transaction was settled.
    /// </summary>
    public class BankEvent
    {
        /// <summary>
        /// 	A unique string identifying this Bank Event
        /// </summary>
        [Required]
        public String BankEventId { get; set; }

        /// <summary>
        /// 	Bank Event Type with possible values AUTH|CHARGE
        /// </summary>
        [Required]
        public String Type { get; set; }

        /// <summary>
        /// 	Timestamp from Bank
        /// </summary>
        public DateTimeOffset? BankEventTimestamp { get; set; }

        /// <summary>
        /// 	Possible values 'APPROVED' | 'REJECTED'
        /// </summary>
        public String Status { get; set; }

        /// <summary>
        /// 	Bank code on response
        /// </summary>
        public String BankResponseCode { get; set; }

        /// <summary>
        /// 	Processor name (e.g. Fdc, Paypal etc.)
        /// </summary>
        public String PaymentProcessor { get; set; }

        /// <summary>
        /// 	Merchant Reference Number, used to identify the transaction from the merchant side
        /// </summary>
        public String MRN { get; set; }

        /// <summary>
        /// 	N/A
        /// </summary>
        public String MID { get; set; }

        /// <summary>
        /// 	Purchase information associated with this Bank Event
        /// </summary>
        [Required]
        public BankEventPurchase Purchase { get; set; }
    }

    /// <summary>
    /// 	Purchase information associated with this Bank Event
    /// </summary>
    public class BankEventPurchase
    {
        /// <summary>
        /// 	A unique string identifying the purchase
        /// </summary>
        [Required]
        public String PurchaseId { get; set; }
    }

    public enum BankEventType
    {
        AUTH,
        CHARGE
    }

    public enum BankStatus
    {
        APPROVED,
        REJECTED
    }
}
