// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Dynamics.FraudProtection.Models.SignupStatusEvent
{
    /// <summary>
    /// 	Enables the merchant to update Fraud Protection about the status of a signup, such as if the signup has been cancelled.
    /// </summary>
    public class SignupStatusEvent
    {
        /// <summary>
        /// 	Signup identifier in merchant system.
        /// </summary>
        [Required]
        public String SignupId { get; set; }

        /// <summary>
        /// 	Status of the signup that the merchant decides
        /// </summary>
        [Required]
        public SignupStatus Status { get; set; }

        public SignupStatusUser User { get; set; }
    }

    /// <summary>
    /// 	Status of a signup that the merchant decides about the signup event.
    /// </summary>
    public class SignupStatus
    {
        /// <summary>
        /// 	The type of the status. Possible values 'APPROVED' | 'REJECTED' | 'HELD'
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

    /// <summary>
    /// 	Status of a signup that the merchant decides about the signup event.
    /// </summary>
    public class SignupStatusUser
    {
        /// <summary>
        /// 	Identifier of the customer, if merchant accepts customer’s sign up request.
        /// </summary>
        [Required]
        public String UserId { get; set; }
    }

    public enum SignupStatusType
    {
        APPROVED,
        REJECTED,
        HELD
    }
}
