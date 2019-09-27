// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;

namespace Microsoft.Dynamics.FraudProtection.Models.LabelEvent
{
    /// <summary>
    /// Provides information about a variety of fraud vectors besides chargebacks and refunds.
    /// </summary>
    public class Label : BaseFraudProtectionEvent
    {
        /// <summary>
        /// Entity type to which this label applies.
        /// </summary>
        public String LabelObjectType { get; set; }

        /// <summary>
        /// The ID value within the LabelObjectType to which this label applies.
        /// </summary>
        public String LabelObjectId { get; set; }

        /// <summary>
        /// The authority issuing this label's LabelReasonCodes or LabelState.
        /// </summary>
        public String LabelSource { get; set; }

        /// <summary>
        /// A comma-delimited list of reasons determined from the LabelSource.
        /// </summary>
        public String LabelReasonCodes { get; set; }

        /// <summary>
        /// If the LabelSource has state-transitions, the current state of this label. Ex. "Accepted" to mean a prior fraud suspicion has been cleared.
        /// </summary>
        public String LabelState { get; set; }

        /// <summary>
        /// The specific LabelSource institution issuing the LabelReasonCodes.
        /// </summary>
        public String Processor { get; set; }

        /// <summary>
        /// The event's ultimate creation date as reported from LabelSource/Processor. Format is ISO8601.
        /// </summary>
        public DateTimeOffset? EventTimeStamp { get; set; }

        /// <summary>
        /// The beginning of when this label applies in the merchant time-zone (if different from MerchantLocalDate). Format is ISO8601.
        /// </summary>
        public DateTimeOffset? EffectiveStartDate { get; set; }

        /// <summary>
        /// The end of when this label applies in merchant time-zone or null for not-applicable/indeterminate. Format is ISO8601.
        /// </summary>
        public DateTimeOffset? EffectiveEndDate { get; set; }

        /// <summary>
        /// The monetary amount this label pertains to, e.g. of a partial refund. Units given by Currency field.
        /// </summary>
        public Decimal Amount { get; set; }

        /// <summary>
        /// Which currency the amount is in, e.g. "USD"
        /// </summary>
        public String Currency { get; set; }
    }

    public enum LabelObjectType
    {
        Purchase,
        Signup,
        GenericEvent,
        Account,
        PI,
        Email
    }

    public enum LabelSource
    {
        CustomerEscalation,
        Chargeback,
        TC40_SAFE,
        ManualReview,
        Refund,
        OfflineAnalysis
    }

    public enum LabelReasonCodes
    {
        FraudRefund,
        AccountTakeOver,
        PIFraud,
        AccountFraud,
        Abuse,
        FriendlyFraud
    }

    public enum LabelState
    {
        Inquiry,
        Received,
        Accepted,
        Fraud,
        Disputed,
        Reversed,
        ResubmittedRequest,
        Abuse
    }
}
