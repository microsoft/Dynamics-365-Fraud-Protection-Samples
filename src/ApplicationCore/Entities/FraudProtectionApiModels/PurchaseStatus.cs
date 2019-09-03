// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Dynamics.FraudProtection.Models.PurchaseStatusEvent
{
    /// <summary>
    /// 	Enables the merchant to update Fraud Protection about the status of a purchase, such as if the purchase has been cancelled.
    /// </summary>
    public class PurchaseStatusEvent : BaseFraudProtectionEvent
    {
        /// <summary>
        /// 	Transaction (or purchase/order) identifier in merchant system.
        /// </summary>
        [Required]
        public String PurchaseId { get; set; }

        /// <summary>
        /// 	The type of the status. Possible values 'APPROVED' | 'CANCELED' | 'HELD' | 'FULFILLED'
        /// </summary>
        [Required]
        public String StatusType { get; set; }

        /// <summary>
        /// 	The DateTime when this status was applied
        /// </summary>
        [Required]
        public DateTimeOffset StatusDate { get; set; }

        /// <summary>
        /// 	Reason of the status transition
        /// </summary>
        public String Reason { get; set; }
    }

    public enum PurchaseStatusType
    {
        Approved,
        Canceled,
        Pending,
        Rejected,
        Failed
    }
}
