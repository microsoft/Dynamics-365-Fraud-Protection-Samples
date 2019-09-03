// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Dynamics.FraudProtection.Models.RefundEvent
{
    /// <summary>
    /// 	Provides information about a previous purchase transaction being refunded.
    /// </summary>
    public class Refund : BaseFraudProtectionEvent
    {
        /// <summary>
        /// 	A unique string identifying this refund event
        /// </summary>
        [Required]
        public String RefundId { get; set; }

        /// <summary>
        /// 	User provided reason
        /// </summary>
        public String Reason { get; set; }

        /// <summary>
        /// 	Possible values INITIATED | COMPLETED
        /// </summary>
        public String Status { get; set; }

        /// <summary>
        /// 	Timestamp from Bank
        /// </summary>
        public DateTimeOffset? BankEventTimestamp { get; set; }

        /// <summary>
        /// 	The refund amount
        /// </summary>
        public Decimal? Amount { get; set; }

        /// <summary>
        /// 	Currency used for sales price amount.
        /// </summary>
        public String Currency { get; set; }

        /// <summary>
        /// 	A unique string identifying the User
        /// </summary>
        [Required]
        public String UserId { get; set; }

        /// <summary>
        /// 	The original Purchase Id
        /// </summary>
        [Required]
        public String PurchaseId { get; set; }
    }

    public enum RefundStatus
    {
        INITIATED,
        COMPLETED,
    }
}
