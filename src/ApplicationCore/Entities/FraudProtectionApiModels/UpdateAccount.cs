// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Dynamics.FraudProtection.Models.UpdateAccountEvent
{
    /// <summary>
    /// 	Updates or creates User account information, such as Add Payment Instrument, Add Address, or any other user attribute.
    /// </summary>
    public class User
    {
        /// <summary>
        /// 	The User Id uniquely identifying the User account in Merchant
        /// </summary>
        [Required]
        public String UserId { get; set; }

        /// <summary>
        /// 	Customer account creation date.
        /// </summary>
        public DateTimeOffset? CreationDate { get; set; }

        /// <summary>
        /// 	Latest date customer data has changed.
        /// </summary>
        public DateTimeOffset? UpdateDate { get; set; }

        /// <summary>
        /// 	Customer-provided first name on customer account.
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// 	Customer-provided last name on customer account.
        /// </summary>
        public String LastName { get; set; }

        /// <summary>
        /// 	Country of customer. 2 alpha country code, e.g., 'US'
        /// </summary>
        public String Country { get; set; }

        /// <summary>
        /// 	Postal code of customer.
        /// </summary>
        public String ZipCode { get; set; }

        /// <summary>
        /// 	Time zone of customer.
        /// </summary>
        public String TimeZone { get; set; }

        /// <summary>
        /// 	Language of customer. Locale, Language-Territory (for example, EN-US).
        /// </summary>
        public String Language { get; set; }

        /// <summary>
        /// 	Phone number of customer. Country code followed by phone number; with the country code and phone number separated by ‘-’ (for example, for US - +1-1234567890).
        /// </summary>
        public String PhoneNumber { get; set; }

        /// <summary>
        /// 	Email of customer. Case insensitive.
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// 	Customer's membership id.
        /// </summary>
        public String MembershipId { get; set; }

        /// <summary>
        /// 	The profile type, possible values: Consumer, Developer, Seller, Publisher, Tenant
        /// </summary>
        public String ProfileType { get; set; }

        /// <summary>
        /// 	The profile name
        /// </summary>
        public String ProfileName { get; set; }

        /// <summary>
        /// 	The authentication provider of the customer, e.g Windows Live, Facebook, Google
        /// </summary>
        public String AuthenticationProvider { get; set; }

        /// <summary>
        /// 	The name displayed on merchant site, e.g. Gamer tag.
        /// </summary>
        public String DisplayName { get; set; }

        /// <summary>
        /// 	If customer-provided email has been verified to be owned by the customer.
        /// </summary>
        public Boolean IsEmailValidated { get; set; }

        /// <summary>
        /// 	Date customer-provided email verified to be owned by the customer.
        /// </summary>
        public DateTimeOffset? EmailValidatedDate { get; set; }

        /// <summary>
        /// 	If customer-provided phone number has been verified to be owned by the customer.
        /// </summary>
        public Boolean IsPhoneNumberValidated { get; set; }

        /// <summary>
        /// 	Date customer-provided phone number has been verified to be owned by the customer.
        /// </summary>
        public DateTimeOffset? PhoneNumberValidatedDate { get; set; }

        /// <summary>
        /// 	Payment instrument associated with this update account event
        /// </summary>
        public List<UserPaymentInstrument> PaymentInstrumentList { get; set; }

        /// <summary>
        /// 	Address associated with this update account event
        /// </summary>
        public List<UserAddress> AddressList { get; set; }

        /// <summary>
        /// 	Device associated with this update account event
        /// </summary>
        public UserDeviceContext DeviceContext { get; set; }
    }

    /// <summary>
    /// 	Payment instrument associated with this update account event
    /// </summary>
    public class UserPaymentInstrument
    {
        /// <summary>
        /// 	Payment instrument details associated with this payment instrument
        /// </summary>
        public PaymentInstrument PaymentInstrumentDetails { get; set; }
    }

    public enum UserAddressType
    {
        SHIPPING,
        BILLING,
        SIGNUP
    }

    /// <summary>
    /// 	Payment instrument details associated with this payment instrument
    /// </summary>
    public class PaymentInstrument
    {
        /// <summary>
        /// 	Identifier for the PI in merchant system, mastered by Merchant.
        /// </summary>
        public String MerchantPaymentInstrumentId { get; set; }

        /// <summary>
        /// 	Type of payment. Possible values 'CREDITCARD', 'DEBITCARD', 'PAYPAL', 'MOBILEPAYMENT', 'GIFTCARD'
        /// </summary>
        [Required]
        public String Type { get; set; }

        /// <summary>
        /// 	Date PI was first entered in merchant system.
        /// </summary>
        public DateTimeOffset? CreationDate { get; set; }

        /// <summary>
        /// 	Date PI was last updated in merchant system.
        /// </summary>
        public DateTimeOffset? UpdateDate { get; set; }

        /// <summary>
        /// 	Defines the state of the PI. Sample: Active, Blocked, Expired
        /// </summary>
        public String State { get; set; }

        /// <summary>
        /// 	For CREDITCARD/DEBITCARD only
        /// </summary>
        public String CardType { get; set; }

        /// <summary>
        /// 	Name of the user of the PI. For CREDITCARD/DEBITCARD only
        /// </summary>
        public String HolderName { get; set; }

        /// <summary>
        /// 	For CREDITCARD/DEBITCARD only
        /// </summary>
        public String BIN { get; set; }

        /// <summary>
        /// 	Date PI was Expired in merchant system. For CREDITCARD/DEBITCARD only
        /// </summary>
        public String ExpirationDate { get; set; }

        /// <summary>
        /// 	For CREDITCARD/DEBITCARD only
        /// </summary>
        public String LastFourDigits { get; set; }

        /// <summary>
        /// 	Email associated with the PI. For PAYPAL only
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// 	For PAYPAL only
        /// </summary>
        public String BillingAgreementId { get; set; }

        /// <summary>
        /// 	For PAYPAL only
        /// </summary>
        public String PayerId { get; set; }

        /// <summary>
        /// 	For PAYPAL only, This indicates if PayPal has verified the payer.
        /// </summary>
        public String PayerStatus { get; set; }

        /// <summary>
        /// 	For PAYPAL only, This indicates if PayPal has verified the payer’s address.
        /// </summary>
        public String AddressStatus { get; set; }

        /// <summary>
        /// 	For MOBILEPAYMENT only
        /// </summary>
        public String IMEI { get; set; }

        /// <summary>
        /// 	Address associated with this payment instrument
        /// </summary>
        public PaymentInstrumentAddress BillingAddress { get; set; }
    }

    /// <summary>
    /// 	Address associated with this payment instrument
    /// </summary>
    public class PaymentInstrumentAddress
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
        /// 	Address details associated with this address
        /// </summary>
        public Address BillingAddressDetails { get; set; }
    }

    /// <summary>
    /// 	Address details associated with this address
    /// </summary>
    public class Address
    {
        /// <summary>
        /// 	First row provided with address.
        /// </summary>
        public String Street1 { get; set; }

        /// <summary>
        /// 	Second row provided with address (may be blank).
        /// </summary>
        public String Street2 { get; set; }

        /// <summary>
        /// 	Third row provided with address (may be blank).
        /// </summary>
        public String Street3 { get; set; }

        /// <summary>
        /// 	City provided with address.
        /// </summary>
        public String City { get; set; }

        /// <summary>
        /// 	State/Region provided with address.
        /// </summary>
        public String State { get; set; }

        /// <summary>
        /// 	District provided with address (may be blank).
        /// </summary>
        public String District { get; set; }

        /// <summary>
        /// 	Zip code provided with address.
        /// </summary>
        public String ZipCode { get; set; }

        /// <summary>
        /// 	ISO country code provided with address. 2 alpha country code, e.g., 'US'
        /// </summary>
        public String Country { get; set; }
    }

    /// <summary>
    /// 	Address associated with this update account event
    /// </summary>
    public class UserAddress
    {
        /// <summary>
        /// 	Possible values 'SHIPPING', 'BILLING', 'SIGNUP'
        /// </summary>
        public String Type { get; set; }

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
        /// 	Address details of this address
        /// </summary>
        public Address AddressDetails { get; set; }
    }

    /// <summary>
    /// 	Device associated with this update account event
    /// </summary>
    public class UserDeviceContext
    {
        /// <summary>
        /// 	Customer's Session Id, or transaction Id if session is not available
        /// </summary>
        [Required]
        public String DeviceContextId { get; set; }

        /// <summary>
        /// 	Customer's IP address (provided by Merchant)
        /// </summary>
        public String IPAddress { get; set; }

        /// <summary>
        /// 	Device details associated with this Device
        /// </summary>
        public DeviceContext DeviceContextDetails { get; set; }
    }

    /// <summary>
    /// 	Device details associated with this Device
    /// </summary>
    public class DeviceContext
    {
        /// <summary>
        /// 	Provider of Device info. Can be one of DFPFINGERPRINTING|MERCHANT. If not specified, default is DFPFINGERPRINTING
        /// </summary>
        public String Provider { get; set; }

        /// <summary>
        /// 	Microsoft device fingerprinting data center for the customer's session Id
        /// </summary>
        public String DeviceContextDC { get; set; }

        /// <summary>
        /// 	Customer's Device Id provided and mastered by Merchant
        /// </summary>
        public String ExternalDeviceId { get; set; }

        /// <summary>
        /// 	Customer's Device Type provided and mastered by Merchant
        /// </summary>
        public String ExternalDeviceType { get; set; }
    }
}
