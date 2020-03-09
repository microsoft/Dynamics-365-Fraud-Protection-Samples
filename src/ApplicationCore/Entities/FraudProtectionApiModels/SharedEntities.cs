// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Dynamics.FraudProtection.Models
{
    /// <summary>
    /// Device information for the assessment/event.
    /// </summary>
    public class DeviceContext
    {
        /// <summary>
        /// Customer's Session Id, or transaction Id if session is not available
        /// </summary>
        [Required]
        public String DeviceContextId { get; set; }

        /// <summary>
        /// Customer's IP address (provided by Merchant)
        /// </summary>
        public String IPAddress { get; set; }

        /// <summary>
        /// Provider of device info. The default is DFPFingerPrinting.
        /// </summary>
        public String Provider { get; set; }

        /// <summary>
        /// Deprecated and no longer needed. Microsoft device fingerprinting datacenter for the customer's session Id
        /// </summary>
        public String DeviceContextDC { get; set; }

        /// <summary>
        /// Customer's Device Id provided and mastered by Merchant
        /// </summary>
        public String ExternalDeviceId { get; set; }

        /// <summary>
        /// The customer's device type, as provided and mastered by the merchant. Possible values Mobile, Computer, MerchantHardware, Tablet, GameConsole
        /// </summary>
        public String ExternalDeviceType { get; set; }
    }

    /// <summary>
    /// User information associated with this event
    /// </summary>
    public class UserDetails
    {
        /// <summary>
        /// A unique string identifying the User
        /// </summary>
        [Required]
        public String UserId { get; set; }

        /// <summary>
        /// Customer account creation date.
        /// </summary>
        public DateTimeOffset? CreationDate { get; set; }

        /// <summary>
        /// Latest date customer data has changed.
        /// </summary>
        public DateTimeOffset? UpdateDate { get; set; }

        /// <summary>
        /// Customer-provided first name on customer account.
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// Customer-provided last name on customer account.
        /// </summary>
        public String LastName { get; set; }

        /// <summary>
        /// Country of customer. 2 alpha country code, e.g., 'US'
        /// </summary>
        public String Country { get; set; }

        /// <summary>
        /// Postal code of customer.
        /// </summary>
        public String ZipCode { get; set; }

        /// <summary>
        /// Time zone of customer.
        /// </summary>
        public String TimeZone { get; set; }

        /// <summary>
        /// Language of customer. Locale, Language-Territory (for example, EN-US).
        /// </summary>
        public String Language { get; set; }

        /// <summary>
        /// Phone number of customer. Country code followed by phone number; with the country code and phone number separated by '-' (for example, for US - +1-1234567890).
        /// </summary>
        public String PhoneNumber { get; set; }

        /// <summary>
        /// Email of customer. Case insensitive.
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// Customer's membership id.
        /// </summary>
        public String MembershipId { get; set; }

        /// <summary>
        /// The profile type. See UserProfileType enum.
        /// </summary>
        public String ProfileType { get; set; }

        /// <summary>
        /// The profile name
        /// </summary>
        public String ProfileName { get; set; }

        /// <summary>
        /// The customers's authentication provider, if different from the merchant's. e.g. If they authenticate via a third party. See AuthenticationProviderType enum.
        /// </summary>
        public String AuthenticationProvider { get; set; }

        /// <summary>
        /// The name displayed on merchant site, e.g. Gamertag.
        /// </summary>
        public String DisplayName { get; set; }

        /// <summary>
        /// If customer-provided email has been verified to be owned by the customer.
        /// </summary>
        public Boolean? IsEmailValidated { get; set; }

        /// <summary>
        /// Date customer-provided email verified to be owned by the customer.
        /// </summary>
        public DateTimeOffset? EmailValidatedDate { get; set; }

        /// <summary>
        /// If customer-provided phone number has been verified to be owned by the customer.
        /// </summary>
        public Boolean? IsPhoneNumberValidated { get; set; }

        /// <summary>
        /// Date customer-provided phone number has been verified to be owned by the customer.
        /// </summary>
        public DateTimeOffset? PhoneNumberValidatedDate { get; set; }
    }

    /// <summary>
    /// Address associated with this event
    /// </summary>
    public class AddressDetails
    {
        /// <summary>
        /// First Name provided with the address
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// Last Name provided with the address
        /// </summary>
        public String LastName { get; set; }

        /// <summary>
        /// Phone Number provided with the address
        /// </summary>
        public String PhoneNumber { get; set; }

        /// <summary>
        /// First row provided with address.
        /// </summary>
        public String Street1 { get; set; }

        /// <summary>
        /// Second row provided with address (may be blank).
        /// </summary>
        public String Street2 { get; set; }

        /// <summary>
        /// Third row provided with address (may be blank).
        /// </summary>
        public String Street3 { get; set; }

        /// <summary>
        /// City provided with address.
        /// </summary>
        public String City { get; set; }

        /// <summary>
        /// State/Region provided with address.
        /// </summary>
        public String State { get; set; }

        /// <summary>
        /// District provided with address (may be blank).
        /// </summary>
        public String District { get; set; }

        /// <summary>
        /// Zip code provided with address.
        /// </summary>
        public String ZipCode { get; set; }

        /// <summary>
        /// ISO country code provided with address. 2 alpha country code, e.g., 'US'
        /// </summary>
        public String Country { get; set; }
    }

    /// <summary>
    /// Payment instrument associated this event
    /// </summary>
    public class PaymentInstrument
    {
        /// <summary>
        /// Identifier for the PI in merchant system, mastered by Merchant.
        /// </summary>
        public String MerchantPaymentInstrumentId { get; set; }

        /// <summary>
        /// Type of payment. See PaymentInstrumentType enum.
        /// </summary>
        [Required]
        public String Type { get; set; }

        /// <summary>
        /// Date PI was first entered in merchant system.
        /// </summary>
        public DateTimeOffset? CreationDate { get; set; }

        /// <summary>
        /// Date PI was last updated in merchant system.
        /// </summary>
        public DateTimeOffset? UpdateDate { get; set; }

        /// <summary>
        /// Defines the state of the PI. See PaymentInstrumentState enum.
        /// </summary>
        public String State { get; set; }

        /// <summary>
        /// The payment type: Examples values Visa, Mastercard, Amex, ACH, SEPA, UnionPay, Inicis, MobileBillingCarrier, Discover, AllPay, JCB, DiscoverDiners
        /// </summary>
        public String CardType { get; set; }

        /// <summary>
        /// Name of the user of the PI. For CREDITCARD/DEBITCARD only
        /// </summary>
        public String HolderName { get; set; }

        /// <summary>
        /// For CREDITCARD/DEBITCARD only
        /// </summary>
        public String BIN { get; set; }

        /// <summary>
        /// Date PI was Expired in merchant system. For CREDITCARD/DEBITCARD only
        /// </summary>
        public String ExpirationDate { get; set; }

        /// <summary>
        /// For CREDITCARD/DEBITCARD only
        /// </summary>
        public String LastFourDigits { get; set; }

        /// <summary>
        /// Email associated with the PI. For PAYPAL only
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// For PAYPAL only
        /// </summary>
        public String BillingAgreementId { get; set; }

        /// <summary>
        /// For PAYPAL only
        /// </summary>
        public String PayerId { get; set; }

        /// <summary>
        /// For PayPal only. A value that indicates whether PayPal has verified the payer. Possible values Verified, Unverified
        /// </summary>
        public String PayerStatus { get; set; }

        /// <summary>
        /// For PayPal only. A value that indicates whether PayPal has verified the payer's address. Possible values Confirmed, Unconfirmed
        /// </summary>
        public String AddressStatus { get; set; }

        /// <summary>
        /// For MOBILEPAYMENT only
        /// </summary>
        public String IMEI { get; set; }

        /// <summary>
        /// Address information associated with this payment instrument
        /// </summary>
        public AddressDetails BillingAddress { get; set; }
    }

    public class MarketingContext
    {
        /// <summary>
        /// The marketing type. See MarketingType enum.
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// The source of this user if known. If via Referral, provide the MerchantUserId of the referrer.
        /// </summary>
        public String TrafficSource { get; set; }

        /// <summary>
        /// The incentive type for the new user. See MarketingIncentiveType enum.
        /// </summary>
        public String IncentiveType { get; set; }

        /// <summary>
        /// The exact incentive offer name. Examples: $5 off on first order, free shipping, 5000 points
        /// </summary>
        public String IncentiveOffer { get; set; }
    }

    public class StoreFrontContext
    {
        /// <summary>
        /// The type of the storefront. See StorefrontType enum.
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// The exact name of your storefront through which the customer is trying to sign up.
        /// </summary>
        public String StoreName { get; set; }

        /// <summary>
        /// The region or market. The value should be a two-letter ISO country/region code (for example, US).
        /// </summary>
        public String Market { get; set; }
    }
}
