// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Dynamics.FraudProtection.Models.PurchaseEvent
{
    /// <summary>
    /// 	Contains information and context about an incoming new purchase transaction for a risk evaluation, including product details.
    /// </summary>
    public class Purchase
    {
        /// <summary>
        /// 	Transaction (or purchase/order) identifier in merchant system.
        /// </summary>
        [Required]
        public String PurchaseId { get; set; }

        /// <summary>
        /// 	Indicates the assessment type for the event. Possible values are 'evaluate' or 'protect'.
        /// </summary>
        public String AssessmentType { get; set; }

        /// <summary>
        /// 	Original Order identifier in merchant system for payments for recurring billing like subscription monthly billing.
        /// </summary>
        public String OriginalOrderId { get; set; }

        /// <summary>
        /// 	Purchase creation date as per Customer local time-zone. Format is ISO8601
        /// </summary>
        public DateTimeOffset? CustomerLocalDate { get; set; }

        /// <summary>
        /// 	Purchase ingestion date in Merchant per Merchant time-zone. Format is ISO8601
        /// </summary>
        [Required]
        public DateTimeOffset MerchantLocalDate { get; set; }

        /// <summary>
        /// 	Total amount charged to the customer; tax included. Provided by the Merchant.
        /// </summary>
        public Decimal? TotalAmount { get; set; }

        /// <summary>
        /// 	Sales tax charged for the transaction. Provided by the Merchant.
        /// </summary>
        public Decimal? SalesTax { get; set; }

        /// <summary>
        /// 	Currency of the original purchase. Provided by the Merchant.
        /// </summary>
        public String Currency { get; set; }

        /// <summary>
        /// 	Indicates the method used to ship the purchase.
        /// </summary>
        public String ShippingMethod { get; set; }

        /// <summary>
        /// 	User information associated with this purchase transaction
        /// </summary>
        public PurchaseUser User { get; set; }

        /// <summary>
        /// 	Purchase Device associated with this purchase transaction
        /// </summary>
        public PurchaseDeviceContext DeviceContext { get; set; }

        /// <summary>
        /// 	Purchase address associated with this purchase transaction
        /// </summary>
        public PurchaseAddress ShippingAddress { get; set; }

        /// <summary>
        /// 	Payment instrument information associated with this purchase transaction
        /// </summary>
        public List<PurchasePaymentInstrument> PaymentInstrumentList { get; set; }

        /// <summary>
        /// 	Product associated with this purchase transaction
        /// </summary>
        public List<PurchaseProduct> ProductList { get; set; }
    }

    /// <summary>
    /// The possible purchase assessment types.
    /// </summary>
    public enum PurchaseAssessmentType
    {
        evaluate,
        protect
    }

    /// <summary>
    /// The possible shipping methods.
    /// </summary>
    public enum PurchaseShippingMethod
    {
        Standard,
        Expedited,
        Overnight
    }

    /// <summary>
    /// 	User information associated with this purchase transaction
    /// </summary>
    public class PurchaseUser
    {
        /// <summary>
        /// 	A unique string identifying the User
        /// </summary>
        [Required]
        public String UserId { get; set; }

        /// <summary>
        /// 	User Details associated with this purchase transaction
        /// </summary>
        public User UserDetails { get; set; }
    }

    /// <summary>
    /// 	User Details associated with this purchase transaction
    /// </summary>
    public class User
    {
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
        /// 	The name displayed on merchant site, e.g. Gamertag.
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
    }

    /// <summary>
    /// 	Purchase Device associated with this purchase transaction
    /// </summary>
    public class PurchaseDeviceContext
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
        /// 	Device details for this device
        /// </summary>
        public DeviceContext DeviceContextDetails { get; set; }
    }

    /// <summary>
    /// 	Device details for this device
    /// </summary>
    public class DeviceContext
    {
        /// <summary>
        /// 	Provider of Device info. Can be one of DFPFINGERPRINTING|MERCHANT. If not specified, default is DFPFINGERPRINTING
        /// </summary>
        public String Provider { get; set; }

        /// <summary>
        /// 	Microsoft device fingerprinting datacenter for the customer's session Id
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

    /// <summary>
    /// 	Purchase address associated with this purchase transaction
    /// </summary>
    public class PurchaseAddress
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
        /// 	Address details associated with this purchase transaction
        /// </summary>
        public Address ShippingAddressDetails { get; set; }
    }

    /// <summary>
    /// 	Address details associated with this purchase transaction
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
    /// 	Payment instrument information associated with this purchase transaction
    /// </summary>
    public class PurchasePaymentInstrument
    {
        /// <summary>
        /// 	Total purchase amount using this PI for the transaction
        /// </summary>
        public Decimal? PurchaseAmount { get; set; }

        /// <summary>
        /// 	Payment instrument associated this purchase transation
        /// </summary>
        public PaymentInstrument PaymentInstrumentDetails { get; set; }
    }

    /// <summary>
    /// 	Payment instrument associated this purchase transation
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
        /// 	Address information associated with this payment instrument
        /// </summary>
        public PaymentInstrumentAddress BillingAddress { get; set; }
    }

    /// <summary>
    /// 	Address information associated with this payment instrument
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
        /// 	Address details associated with this payment instrument
        /// </summary>
        [Required]
        public Address BillingAddressDetails { get; set; }
    }

    /// <summary>
    /// 	Product associated with this purchase transaction
    /// </summary>
    public class PurchaseProduct
    {
        /// <summary>
        /// 	Unique identifier of this product
        /// </summary>
        [Required]
        public String ProductId { get; set; }

        /// <summary>
        /// 	Price for line item at the purchase.
        /// </summary>
        public Decimal? PurchasePrice { get; set; }

        /// <summary>
        /// 	Margin gained by sale of item.
        /// </summary>
        public Decimal? Margin { get; set; }

        /// <summary>
        /// 	Quantity of item purchased
        /// </summary>
        public Int32? Quantity { get; set; }

        /// <summary>
        /// 	Indicates if the product is offered for preorder.
        /// </summary>
        public Boolean IsPreorder { get; set; }

        /// <summary>
        /// 	Indicates the method used to ship the product.
        /// </summary>
        public String ShippingMethod { get; set; }

        /// <summary>
        /// 	Product details associated with this product
        /// </summary>
        public Product ProductDetails { get; set; }
    }

    /// <summary>
    /// 	Product details associated with this product
    /// </summary>
    public class Product
    {
        /// <summary>
        /// 	User-readable product name.
        /// </summary>
        public String ProductName { get; set; }

        /// <summary>
        /// 	Type of product sold (physical good, digital subscription, etc.).
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 	Product SKU
        /// </summary>
        public String Sku { get; set; }

        /// <summary>
        /// 	Category of product (Apparel, Shoes, Accessories).
        /// </summary>
        public String Category { get; set; }

        /// <summary>
        /// 	Market in which product is offered
        /// </summary>
        public String Market { get; set; }

        /// <summary>
        /// 	Price of item sold (not including tax). Provided by the Merchant.
        /// </summary>
        public Decimal? SalesPrice { get; set; }

        /// <summary>
        /// 	Currency used for sales price. Provided by the Merchant.
        /// </summary>
        public String Currency { get; set; }

        /// <summary>
        /// 	Cost of Goods Sold – raw material cost of item. Provided by the Merchant.
        /// </summary>
        public Decimal? COGS { get; set; }

        /// <summary>
        /// 	Indicates if product is recurring subscription.
        /// </summary>
        public Boolean IsRecurring { get; set; }

        /// <summary>
        /// 	Indicates if product is offered for free.
        /// </summary>
        public Boolean IsFree { get; set; }

        /// <summary>
        /// 	Language in which product is described.
        /// </summary>
        public String Language { get; set; }
    }
}
