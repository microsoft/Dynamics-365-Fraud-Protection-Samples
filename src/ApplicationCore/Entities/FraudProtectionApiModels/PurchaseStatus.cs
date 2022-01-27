// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Dynamics.FraudProtection.Models.PurchaseStatusEvent
{
    /// <summary>
    /// Enables the merchant to update Fraud Protection about the status of a purchase, such as if the purchase has been cancelled.
    /// </summary>
    public class PurchaseStatusEvent : BaseFraudProtectionEvent
    {
        /// <summary>
        /// Transaction (or purchase/order) identifier in merchant system.
        /// </summary>
        [Required]
        public String PurchaseId { get; set; }

        /// <summary>
        /// The purchase status. See PurchaseStatusType enum.
        /// </summary>
        [Required]
        public String StatusType { get; set; }

        /// <summary>
        /// The DateTime when this status was applied
        /// </summary>
        [Required]
        public DateTimeOffset StatusDate { get; set; }

        /// <summary>
        /// Reason of the status transition. See PurchaseStatusReason enum.
        /// </summary>
        public String Reason { get; set; }
    }

    public enum PurchaseStatusType
    {
        Approved,
        Pending,
        Rejected,
        Failed,
        Canceled,
        Unknown
    }

    public enum PurchaseStatusReason
    {
        RuleEngine,
        MerchantOverride,
        ChallengeSolved,
        ChallengeFailed,
        CustomerRequest,
        FufillmentFailed,
        InlineManualReview_General,
        InlineManualReview_Fraud,
        InlineManualReview_AccountCompromise,
        OfflineManualReview_General,
        OfflineManualReview_Fraud,
        OfflineManualReview_AccountCompromise,
        Safelist,
        Blocklist
    }
}
