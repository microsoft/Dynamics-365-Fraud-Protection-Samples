// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.OrderAggregate;
using System;
using System.Collections.Generic;

namespace Contoso.FraudProtection.Web.ViewModels
{
    public class OrderViewModel
    {
        public int OrderNumber { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }

        public Address ShippingAddress { get; set; }

        public bool AllowReturn { get; set; }
        public bool AllowChargeback { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();

        public string ReturnOrChargebackReason { get; set; }
    }

}
