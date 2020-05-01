// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Ardalis.GuardClauses;
using Microsoft.Dynamics.FraudProtection.Models.PurchaseEvent;
using Microsoft.Dynamics.FraudProtection.Models.RefundEvent;
using Microsoft.Dynamics.FraudProtection.Models.ChargebackEvent;
using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels;
using Contoso.FraudProtection.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Contoso.FraudProtection.ApplicationCore.Services;
using System.Linq;
using System.Text.Json;

namespace Contoso.FraudProtection.ApplicationCore.Entities.OrderAggregate
{
    public class Order : BaseEntity, IAggregateRoot
    {
        public Order()
        {
            // required by EF
        }

        public Order(
            string buyerId,
            Address shipToAddress, 
            PaymentInfo paymentDetails,
            List<OrderItem> items, 
            OrderStatus status,
            Purchase purchase, 
            PurchaseResponse purchaseResponse,
            OrderTotals totals)
        {
            Guard.Against.NullOrEmpty(buyerId, nameof(buyerId));
            Guard.Against.Null(shipToAddress, nameof(shipToAddress));
            Guard.Against.Null(items, nameof(items));
            Guard.Against.Null(paymentDetails, nameof(paymentDetails));
            Guard.Against.Null(purchase, nameof(purchase));

            BuyerId = buyerId;
            ShipToAddress = shipToAddress;
            PaymentDetails = paymentDetails;
            Status = status;
            _orderItems = items;
            RiskPurchase = purchase;
            RiskPurchaseResponse = purchaseResponse;
            SubTotal = totals.SubTotal;
            Tax = totals.Tax;
            Total = totals.Total;
        }

        public string BuyerId { get; private set; }

        public DateTimeOffset OrderDate { get; private set; } = DateTimeOffset.Now;
        public Address ShipToAddress { get; private set; }
        public PaymentInfo PaymentDetails { get; private set; }
        public OrderStatus Status { get; set; }

        private readonly List<OrderItem> _orderItems = new List<OrderItem>();
        private string _riskPurchase;
        private string _riskRefund;
        private string _riskRefundResponse;
        private string _riskPurchaseResponse;
        private string _riskChargeback;
        private string _riskChargebackResponse;

        [NotMapped]
        public Purchase RiskPurchase
        {
            get
            {
                return string.IsNullOrEmpty(_riskPurchase) ? null : JsonSerializer.Deserialize<Purchase>(_riskPurchase);
            }
            set
            {
                _riskPurchase = value != null ? JsonSerializer.Serialize(value) : null;
            }
        }

        [NotMapped]
        public Refund RiskRefund
        {
            get
            {
                return string.IsNullOrEmpty(_riskRefund) ? null : JsonSerializer.Deserialize<Refund>(_riskRefund);
            }
            set
            {
                _riskRefund = value != null ? JsonSerializer.Serialize(value) : null;
            }
        }

        [NotMapped]
        public FraudProtectionResponse RiskRefundResponse
        {
            get
            {
                return string.IsNullOrEmpty(_riskRefundResponse) ? null : JsonSerializer.Deserialize<FraudProtectionResponse>(_riskRefundResponse);
            }
            set
            {
                _riskRefundResponse = value != null ? JsonSerializer.Serialize(value) : null;
            }
        }

        [NotMapped]
        public Chargeback RiskChargeback
        {
            get
            {
                return string.IsNullOrEmpty(_riskChargeback) ? null : JsonSerializer.Deserialize<Chargeback>(_riskChargeback);
            }
            set
            {
                _riskChargeback = value != null ? JsonSerializer.Serialize(value) : null;
            }
        }

        [NotMapped]
        public FraudProtectionResponse RiskChargebackResponse
        {
            get
            {
                return string.IsNullOrEmpty(_riskChargebackResponse) ? null : JsonSerializer.Deserialize<FraudProtectionResponse>(_riskChargebackResponse);
            }
            set
            {
                _riskChargebackResponse = value != null ? JsonSerializer.Serialize(value) : null;
            }
        }

        [NotMapped]
        public PurchaseResponse RiskPurchaseResponse
        {
            get
            {
                return string.IsNullOrEmpty(_riskPurchaseResponse) ? null : JsonSerializer.Deserialize<PurchaseResponse>(_riskPurchaseResponse);
            }
            set
            {
                _riskPurchaseResponse = value != null ? JsonSerializer.Serialize(value) : null;
            }
        }

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public string ReturnOrChargebackReason { get; set; }

        public string AdminComments { get; set; }

        [NotMapped]
        public bool AllowReturn =>
            Status == OrderStatus.Received ||
            Status == OrderStatus.InProgress ||
            Status == OrderStatus.Complete;

        [NotMapped]
        public bool AllowChargeback =>
            Status == OrderStatus.Received ||
            Status == OrderStatus.Complete;

        [Column(TypeName = "Money")]
        public decimal SubTotal { get; private set; }
        [Column(TypeName = "Money")]
        public decimal Tax { get; private set; }
        [Column(TypeName = "Money")]
        public decimal Total { get; private set; }
    }
}
