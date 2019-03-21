// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Dynamics.FraudProtection.Models.PurchaseEvent;
using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels;
using Contoso.FraudProtection.ApplicationCore.Entities.OrderAggregate;
using System.Threading.Tasks;

namespace Contoso.FraudProtection.ApplicationCore.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(
            int basketId,
            Entities.OrderAggregate.Address shippingAddress, 
            PaymentInfo paymentDetails, 
            OrderStatus status, 
            Purchase purchase, 
            PurchaseResponse purchaseResponse);
    }
}
