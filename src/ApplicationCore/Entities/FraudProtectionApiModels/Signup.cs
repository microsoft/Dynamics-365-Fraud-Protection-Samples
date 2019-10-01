// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Dynamics.FraudProtection.Models.SignupEvent
{
    /// <summary>
    /// Contains information and context about a signup attempt.
    /// </summary>
    public class SignUp : BaseFraudProtectionEvent
    {
        /// <summary>
        /// The identifier of the Signup event (can match trackingId).
        /// </summary>
        [Required]
        public String SignUpId { get; set; }

        /// <summary>
        /// Indicates the assessment type for the event. See AssessmentType enum. If not specified, default is 'Protect'
        /// </summary>
        public String AssessmentType { get; set; }

        /// <summary>
        /// The Signup creation date in the customer's local time zone. The format is ISO 8601.
        /// </summary>
        public DateTimeOffset? CustomerLocalDate { get; set; }

        /// <summary>
        /// The Signup ingestion date in the merchant's time zone. The format is ISO 8601.
        /// </summary>
        [Required]
        public DateTimeOffset MerchantLocalDate { get; set; }

        /// <summary>
        /// User information associated with this signup
        /// </summary>
        [Required]
        public SignupUser User { get; set; }

        /// <summary>
        /// Marketing based, contextual information related to this signup.
        /// </summary>
        public MarketingContext MarketingContext { get; set; }

        /// <summary>
        /// Store front based, contextual information related to this signup.
        /// </summary>
        public StoreFrontContext StoreFrontContext { get; set; }

        /// <summary>
        /// Purchase Device associated with this purchase transaction
        /// </summary>
        public DeviceContext DeviceContext { get; set; }
    }

    public class SignupUser : UserDetails
    {
        /// <summary>
        /// The address the user entered during signup.
        /// </summary>
        public AddressDetails Address { get; set; }

        /// <summary>
        /// Payment instrument information associated with this signUp transaction
        /// </summary>
        public PaymentInstrument PaymentInstrument { get; set; }
    }
}
