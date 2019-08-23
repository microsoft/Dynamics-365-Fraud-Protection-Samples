// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Dynamics.FraudProtection.Models.SharedEntities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Dynamics.FraudProtection.Models.SignupEvent
{
    /// <summary>
    /// 	Contains information and context about a signup attempt.
    /// </summary>
    public class SignUp
    {
        /// <summary>
        /// 	The identifier of the Signup event (can match trackingId).
        /// </summary>
        [Required]
        public String SignUpId { get; set; }

        /// <summary>
        /// 	Indicates the assessment type for the event. Possible values are 'evaluate' or 'protect'. If not specified, default is 'protect'
        /// </summary>
        public String AssessmentType { get; set; }

        /// <summary>
        /// 	The Signup creation date in the customer's local time zone. The format is ISO 8601.
        /// </summary>
        public DateTimeOffset CustomerLocalDate { get; set; }

        /// <summary>
        /// 	The Signup ingestion date in the merchant's time zone. The format is ISO 8601.
        /// </summary>
        [Required]
        public DateTimeOffset MerchantLocalDate { get; set; }

        /// <summary>
        /// 	User information associated with this signup
        /// </summary>
        [Required]
        public User<SignupUserDetails> User { get; set; }

        /// <summary>
        /// 	N/A
        /// </summary>
        public MarketingContext MarketingContext { get; set; }

        /// <summary>
        /// 	N/A
        /// </summary>
        public StoreFrontContext StoreFrontContext { get; set; }

        /// <summary>
        /// 	Purchase Device associated with this purchase transaction
        /// </summary>
        public DeviceContext DeviceContext { get; set; }
    }

    public class SignupUserDetails : UserDetails
    {
        /// <summary>
        /// 	N/A
        /// </summary>
        public SignupAddress SignUpAddress { get; set; }

        /// <summary>
        /// 	Payment instrument information associated with this signUp transaction
        /// </summary>
        public PaymentInstrument PaymentInstrument { get; set; }
    }

    /// <summary>
    /// 	Address associated with this signup event.
    /// </summary>
    public class SignupAddress
    {
        /// <summary>
        /// 	First Name provided with the address
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// 	Last Name provided with the address
        /// </summary>
        public String LastName { get; set; }

        /// <summary>
        /// 	Phone Number provided with the address
        /// </summary>
        public String PhoneNumber { get; set; }

        /// <summary>
        /// 	Address details associated with this event
        /// </summary>
        public AddressDetails SignupAddressDetails { get; set; }
    }
}
