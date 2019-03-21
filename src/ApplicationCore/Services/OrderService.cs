// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Ardalis.GuardClauses;
using Microsoft.Dynamics.FraudProtection.Models.PurchaseEvent;
using Contoso.FraudProtection.ApplicationCore.Entities;
using Contoso.FraudProtection.ApplicationCore.Entities.BasketAggregate;
using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels;
using Contoso.FraudProtection.ApplicationCore.Entities.OrderAggregate;
using Contoso.FraudProtection.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Contoso.FraudProtection.ApplicationCore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IAsyncRepository<Order> _orderRepository;
        private readonly IAsyncRepository<Basket> _basketRepository;
        private readonly IAsyncRepository<CatalogItem> _itemRepository;

        public OrderService(IAsyncRepository<Basket> basketRepository,
            IAsyncRepository<CatalogItem> itemRepository,
            IAsyncRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
            _basketRepository = basketRepository;
            _itemRepository = itemRepository;
        }

        public async Task CreateOrderAsync(
            int basketId,
            Entities.OrderAggregate.Address shippingAddress,
            PaymentInfo paymentDetails, 
            OrderStatus status,
            Purchase purchase, 
            PurchaseResponse purchaseResponse)
        {
            var basket = await _basketRepository.GetByIdAsync(basketId);
            
            Guard.Against.NullBasket(basketId, basket);
            var items = new List<OrderItem>();
            var pricesAndQuantities = new List<Tuple<decimal, int>>();
            foreach (var item in basket.Items)
            {
                var catalogItem = await _itemRepository.GetByIdAsync(item.CatalogItemId);
                var itemOrdered = new CatalogItemOrdered(catalogItem.Id, catalogItem.Name, catalogItem.PictureUri);
                var orderItem = new OrderItem(itemOrdered, item.UnitPrice, item.Quantity);
                items.Add(orderItem);
                pricesAndQuantities.Add(new Tuple<decimal, int>(item.UnitPrice, item.Quantity));
            }
            var totals = OrderCalculator.CalculateTotals(pricesAndQuantities);

            var order = new Order(
                basket.BuyerId,
                shippingAddress,
                paymentDetails,
                items,
                status,
                purchase, 
                purchaseResponse,
                totals);

            await _orderRepository.AddAsync(order);
        }
    }
}
