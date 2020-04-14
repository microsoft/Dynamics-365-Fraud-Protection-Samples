// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Dynamics.FraudProtection.Models.PurchaseEvent
{
    /// <summary>
    /// Contains information and context about an incoming new purchase transaction for a risk evaluation, including product details.
    /// </summary>
    public class Purchase : BaseFraudProtectionEvent
    {
        /// <summary>
        /// Transaction (or purchase/order) identifier in merchant system.
        /// </summary>
        [Required]
        public String PurchaseId { get; set; }

        /// <summary>
        /// Indicates the assessment type for the event. See AssessmentType enum.
        /// </summary>
        public String AssessmentType { get; set; }

        /// <summary>
        /// Original Order identifier in merchant system for payments for recurring billing like subscription monthly billing.
        /// </summary>
        public String OriginalOrderId { get; set; }

        /// <summary>
        /// Purchase creation date as per Customer local time-zone. Format is ISO8601
        /// </summary>
        public DateTimeOffset? CustomerLocalDate { get; set; }

        /// <summary>
        /// Purchase ingestion date in Merchant per Merchant time-zone. Format is ISO8601
        /// </summary>
        [Required]
        public DateTimeOffset MerchantLocalDate { get; set; }

        /// <summary>
        /// Total amount charged to the customer; tax included. Provided by the Merchant.
        /// </summary>
        public Decimal? TotalAmount { get; set; }

        /// <summary>
        /// Sales tax charged for the transaction. Provided by the Merchant.
        /// </summary>
        public Decimal? SalesTax { get; set; }

        /// <summary>
        /// Currency of the original purchase. Provided by the Merchant.
        /// </summary>
        public String Currency { get; set; }

        /// <summary>
        /// Indicates the method used to ship the purchase.
        /// </summary>
        public String ShippingMethod { get; set; }

        /// <summary>
        /// User information associated with this purchase transaction
        /// </summary>
        public UserDetails User { get; set; }

        /// <summary>
        /// Purchase Device associated with this purchase transaction
        /// </summary>
        public DeviceContext DeviceContext { get; set; }

        /// <summary>
        /// Purchase address associated with this purchase transaction
        /// </summary>
        public AddressDetails ShippingAddress { get; set; }

        /// <summary>
        /// Payment instrument information associated with this purchase transaction
        /// </summary>
        public List<PurchasePaymentInstrument> PaymentInstrumentList { get; set; }

        /// <summary>
        /// Product associated with this purchase transaction
        /// </summary>
        public List<Product> ProductList { get; set; }

        /// <summary>
        /// Optional property bag for any custom data to be used in rules
        /// </summary>
        public Dictionary<string, object> CustomData { get; set; }
    }

    /// <summary>
    /// The possible shipping methods.
    /// </summary>
    public enum PurchaseShippingMethod
    {
        Standard,
        Express,
        InStorePickup,
        DirectEntitlement,
        DigitalToken
    }

    public class PurchasePaymentInstrument : PaymentInstrument
    {
        /// <summary>
        /// Total purchase amount using this PI for the transaction
        /// </summary>
        public Decimal? PurchaseAmount { get; set; }
    }

    /// <summary>
    /// Product associated with this purchase transaction
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Unique identifier of this product
        /// </summary>
        [Required]
        public String ProductId { get; set; }

        /// <summary>
        /// User-readable product name.
        /// </summary>
        public String ProductName { get; set; }

        /// <summary>
        /// Type of product sold (physical good, digital subscription, etc.).
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// Product SKU
        /// </summary>
        public String Sku { get; set; }

        /// <summary>
        /// Category of product (Apparel, Shoes, Accessories).
        /// </summary>
        public String Category { get; set; }

        /// <summary>
        /// Market in which product is offered
        /// </summary>
        public String Market { get; set; }

        /// <summary>
        /// Price of item sold (not including tax). Provided by the Merchant.
        /// </summary>
        public Decimal? SalesPrice { get; set; }

        /// <summary>
        /// Currency used for sales price. Provided by the Merchant.
        /// </summary>
        public String Currency { get; set; }

        /// <summary>
        /// Cost of Goods Sold – raw material cost of item. Provided by the Merchant.
        /// </summary>
        public Decimal? COGS { get; set; }

        /// <summary>
        /// Indicates if product is recurring subscription.
        /// </summary>
        public Boolean IsRecurring { get; set; }

        /// <summary>
        /// Indicates if product is offered for free.
        /// </summary>
        public Boolean IsFree { get; set; }

        /// <summary>
        /// Language in which product is described.
        /// </summary>
        public String Language { get; set; }

        /// <summary>
        /// Price for line item at the purchase.
        /// </summary>
        public Decimal? PurchasePrice { get; set; }

        /// <summary>
        /// Margin gained by sale of item.
        /// </summary>
        public Decimal? Margin { get; set; }

        /// <summary>
        /// Quantity of item purchased
        /// </summary>
        public Int32? Quantity { get; set; }

        /// <summary>
        /// Indicates if the product is offered for preorder.
        /// </summary>
        public Boolean? IsPreorder { get; set; }

        /// <summary>
        /// Indicates the method used to ship the product.
        /// </summary>
        public String ShippingMethod { get; set; }
    }

    /// <summary>
    /// The possible product types
    /// </summary>
    public enum ProductType
    {
        Digital,
        Physical
    }

    /// <summary>
    /// The possible product categories
    /// </summary>
    public enum ProductCategory
    {
        Subscription,
        Game,
        GameConsumable,
        GameDLC,
        HardwareDevice,
        HardwareAccessory,
        SoftwareToken,
        SoftwareDirectEntitlement,
        ClothingShoes,
        RecreationalEquipment,
        Jewelry,
        Hotel,
        Ticket,
        VehicleRental,
        GiftCard,
        Movies,
        Music,
        GarageIndustrial,
        HomeGarden,
        Tools,
        Books,
        HealthBeauty,
        Furniture,
        Toys,
        FoodGrocery
    }
}
