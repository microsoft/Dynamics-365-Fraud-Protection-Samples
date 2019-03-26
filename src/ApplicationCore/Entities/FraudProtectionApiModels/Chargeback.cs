// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Dynamics.FraudProtection.Models.ChargebackEvent
{
    /// <summary>
    /// 	Provides information about a previous purchase that the customer disputed with their bank as fraud.
    /// </summary>
    public class Chargeback
    {
        /// <summary>
        /// 	A unique string identifying this chargeback event
        /// </summary>
        [Required]
        public String ChargebackId { get; set; }

        /// <summary>
        /// 	Reason provided by bank
        /// </summary>
        public String Reason { get; set; }

        /// <summary>
        /// 	Possible values INITIATED | LOST | WON )
        /// </summary>
        public String Status { get; set; }

        /// <summary>
        /// 	Timestamp from Bank
        /// </summary>
        public DateTimeOffset? BankEventTimestamp { get; set; }

        /// <summary>
        /// 	The chargeback amount
        /// </summary>
        public Decimal? Amount { get; set; }

        /// <summary>
        /// 	Currency used for chargeback amount.
        /// </summary>
        public String Currency { get; set; }

        /// <summary>
        /// 	User associated with this chargeback event
        /// </summary>
        public ChargebackUser User { get; set; }

        /// <summary>
        /// 	Purchase information associated with this chargeback event
        /// </summary>
        [Required]
        public ChargebackPurchase Purchase { get; set; }
    }

    public enum ChargebackStatus
    {
        INITIATED,
        LOST,
        WON
    }

    /// <summary>
    /// 	User associated with this chargeback event
    /// </summary>
    public class ChargebackUser
    {
        /// <summary>
        /// 	A unique string identifying the User
        /// </summary>
        [Required]
        public String UserId { get; set; }
    }

    /// <summary>
    /// 	Purchase information associated with this chargeback event
    /// </summary>
    public class ChargebackPurchase
    {
        /// <summary>
        /// 	The original Purchase Id
        /// </summary>
        [Required]
        public String PurchaseId { get; set; }
    }
}
