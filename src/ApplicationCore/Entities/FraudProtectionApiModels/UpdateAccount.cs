// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Dynamics.FraudProtection.Models.SharedEntities;
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
        public DeviceContext DeviceContext { get; set; }
    }

    /// <summary>
    /// 	Payment instrument associated with this update account event
    /// </summary>
    public class UserPaymentInstrument
    {
        /// <summary>
        /// 	Payment instrument details associated with this payment instrument
        /// </summary>
        public PaymentInstrumentDetails PaymentInstrumentDetails { get; set; }
    }

    public enum UserAddressType
    {
        SHIPPING,
        BILLING,
        SIGNUP
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
        public AddressDetails AddressDetails { get; set; }
    }
}
