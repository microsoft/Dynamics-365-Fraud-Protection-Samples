using Microsoft.Dynamics.FraudProtection.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels
{
    /// <summary>
    /// Contains information and context about a sign-in attempt.
    /// </summary>
    public class SignIn: BaseFraudProtectionEvent
    {
        /// <summary>
        /// 	The identifier of the sign-in attempt (can match trackingId).
        /// </summary>
        [Required]
        public string SignInId { get; set; }

        /// <summary>
        /// 	Hash of the user password from Merchant sign-in attempt.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// 	The customer’s IP address (as provided by the merchant) of current sign-in attempt.
        /// </summary>
        public string CurrentIpAddress { get; set; }

        /// <summary>
        /// 	Indicates the assessment type for the event. Possible values are 'Evaluate' or 'Protect'. If not specified, default is 'Protect'.
        /// </summary>
        public string AssessmentType { get; set; }

        /// <summary>
        /// 	The sign-in attempt date in the customer's local time zone. The format is ISO 8601.
        /// </summary>
        public DateTimeOffset CustomerLocalDate { get; set; }

        /// <summary>
        /// 	The sign-in attempt date in the merchant's time zone. The format is ISO 8601.
        /// </summary>
        [Required]
        public DateTimeOffset MerchantLocalDate { get; set; }

        /// <summary>
        /// 	Identifier of user from the merchant.
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// 	Customer's Session Id (mastered by DFP Fingerprinting).
        /// </summary>
        public string DeviceContextId { get; set; }

        /// <summary>
        /// 	N/A
        /// </summary>
        public SignInHistory SignInHistory { get; set; }

    }

    /// <summary>
    /// 	N/A
    /// </summary>
    public class SignInHistory
    {
        /// <summary>
        /// 	IP Address used by customer (provided by Merchant) while signing up with merchant.
        /// </summary>
        public string SignUpIpAddress { get; set; }

        /// <summary>
        /// 	IP Address used by customer (provided by Merchant) for last successful sign-in.
        /// </summary>
        public string LastSignInIpAddress { get; set; }

        /// <summary>
        /// 	IP Address used frequently by customer (provided by Merchant) for previous sign-ins.
        /// </summary>
        public string FrequentSignInIpAddress { get; set; }

        /// <summary>
        /// 	Failed sign-in attempts for current customer UserId or Account (provided by merchant) in last one hour.
        /// </summary>
        public int FailedSignInAttemptsInLastHour { get; set; }

        /// <summary>
        /// 	Total failed sign-in attempts from current customer IP address (provided by merchant) in last one hour.
        /// </summary>
        public int FailedSignInAttemptsFromCurrentIpAddress { get; set; }
    }

}