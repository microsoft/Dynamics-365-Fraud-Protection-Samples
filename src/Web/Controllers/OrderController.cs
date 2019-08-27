// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.OrderAggregate;
using Contoso.FraudProtection.ApplicationCore.Interfaces;
using Contoso.FraudProtection.ApplicationCore.Specifications;
using Contoso.FraudProtection.Infrastructure.Identity;
using Contoso.FraudProtection.Web.Extensions;
using Contoso.FraudProtection.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Dynamics.FraudProtection.Models.ChargebackEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.FraudProtection.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFraudProtectionService _fraudProtectionService;

        public OrderController(IOrderRepository orderRepository, UserManager<ApplicationUser> userManager, IFraudProtectionService fraudProtectionService)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
            _fraudProtectionService = fraudProtectionService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await GetOrders();

            return View(orders.Select(BuildOrderViewModel));
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> Detail(int orderId)
        {
            return await OrderDetailView("Detail", orderId);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> Return(int orderId)
        {
            return await OrderDetailView("Return", orderId);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> ChargeBack(int orderId)
        {
            return await OrderDetailView("ChargeBack", orderId);
        }

        public async Task<IActionResult> ReturnOrder(OrderViewModel viewModel)
        {
            return await ModifyOrderView("ReturnInitiated", viewModel, order =>
            {
                order.ReturnOrChargebackReason = viewModel.ReturnOrChargebackReason;
                order.Status = OrderStatus.ReturnInitiated;

                return Task.CompletedTask;
            });
        }

        public async Task<IActionResult> ChargebackOrder(OrderViewModel viewModel)
        {
            return await ModifyOrderView("ChargebackDone", viewModel, async order =>
            {
                #region Fraud Protection Service
                var chargeback = new Chargeback
                {
                    ChargebackId = Guid.NewGuid().ToString(),
                    Amount = order.Total,
                    Currency = order.RiskPurchase?.Currency,
                    BankEventTimestamp = DateTimeOffset.Now,
                    Reason = viewModel.ReturnOrChargebackReason,
                    Status = ChargebackStatus.WON.ToString(),
                    Purchase = new ChargebackPurchase { PurchaseId = order.RiskPurchase.PurchaseId },
                    User = new ChargebackUser { UserId = order.RiskPurchase.User.UserId },
                };
                var response = await _fraudProtectionService.PostChargeback(chargeback);

                var fraudProtectionIO = new FraudProtectionIOModel(chargeback, response, "Chargeback");
                TempData.Put(FraudProtectionIOModel.TempDataKey, fraudProtectionIO);
                #endregion

                order.ReturnOrChargebackReason = viewModel.ReturnOrChargebackReason;
                order.Status = OrderStatus.ChargeBack;
                order.RiskChargeback = chargeback;
                order.RiskChargebackResponse = response;
            });
        }

        private async Task<IActionResult> OrderDetailView(string viewName, int orderId)
        {
            var orders = await GetOrders();
            var order = orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return BadRequest("No such order found for this user.");
            }

            return View(viewName, BuildOrderViewModel(order));
        }

        private async Task<IActionResult> ModifyOrderView(
            string viewName,
            OrderViewModel viewModel,
            Func<Order, Task> modifyOrder)
        {
            var orders = await GetOrders();
            var order = orders.FirstOrDefault(o => o.Id == viewModel.OrderNumber);
            if (order == null)
            {
                return BadRequest("No such order found for this user.");
            }

            await modifyOrder(order);

            await _orderRepository.UpdateAsync(order);

            return View(viewName);
        }

        private async Task<List<Order>> GetOrders()
        {
            return await _orderRepository.ListAsync(new CustomerOrdersWithItemsSpecification(User.Identity.Name));
        }

        private OrderViewModel BuildOrderViewModel(Order order)
        {
            return new OrderViewModel
            {
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems?.Select(oi => new OrderItemViewModel
                {
                    Discount = 0,
                    PictureUrl = oi.ItemOrdered.PictureUri,
                    ProductId = oi.ItemOrdered.CatalogItemId,
                    ProductName = oi.ItemOrdered.ProductName,
                    UnitPrice = oi.UnitPrice,
                    Units = oi.Units
                }).ToList(),
                OrderNumber = order.Id,
                ShippingAddress = order.ShipToAddress,
                Status = order.Status.GetDescription(),
                Tax = order.Tax,
                Total = order.Total,
                AllowReturn = order.AllowReturn,
                AllowChargeback = order.AllowChargeback,
                ReturnOrChargebackReason = order.ReturnOrChargebackReason
            };
        }
    }
}
