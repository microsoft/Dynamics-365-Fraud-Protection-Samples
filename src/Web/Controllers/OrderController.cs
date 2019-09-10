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
using Microsoft.Dynamics.FraudProtection.Models.LabelEvent;
using Microsoft.Dynamics.FraudProtection.Models.RefundEvent;
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
        private readonly IUriComposer _uriComposer;

        public OrderController(
            IOrderRepository orderRepository,
            UserManager<ApplicationUser> userManager,
            IFraudProtectionService fraudProtectionService,
            IUriComposer uriComposer)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
            _fraudProtectionService = fraudProtectionService;
            _uriComposer = uriComposer;
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

        [HttpGet("{orderId}")]
        public async Task<IActionResult> FraudLabel(int orderId)
        {
            return await OrderDetailView("FraudLabel", orderId);
        }

        [HttpPost]
        public async Task<IActionResult> ReturnOrder(OrderViewModel viewModel, RefundStatus status, RefundReason reason)
        {
            return await ModifyOrderView("ReturnDone", viewModel, async order =>
            {
                #region Fraud Protection Service
                var refund = new Refund
                {
                    RefundId = Guid.NewGuid().ToString(),
                    Amount = order.Total,
                    Currency = order.RiskPurchase?.Currency,
                    BankEventTimestamp = DateTimeOffset.Now,
                    PurchaseId = order.RiskPurchase.PurchaseId,
                    Reason = reason.ToString(),
                    Status = status.ToString(),
                    UserId = order.RiskPurchase.User.UserId,
                };

                var response = await _fraudProtectionService.PostRefund(refund);

                var fraudProtectionIO = new FraudProtectionIOModel(refund, response, "Refund");
                TempData.Put(FraudProtectionIOModel.TempDataKey, fraudProtectionIO);
                #endregion

                order.ReturnOrChargebackReason = reason.ToString();
                order.Status = status == RefundStatus.Approved ? OrderStatus.ReturnCompleted : OrderStatus.ReturnRejected;
                order.RiskRefund = refund;
                order.RiskRefundResponse = response;
            });
        }

        [HttpPost]
        public async Task<IActionResult> ChargebackOrder(OrderViewModel viewModel, ChargebackStatus status)
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
                    Status = status.ToString(),
                    PurchaseId = order.RiskPurchase.PurchaseId,
                    UserId = order.RiskPurchase.User.UserId,
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

        [HttpPost]
        public async Task<IActionResult> FraudLabelOrder(
            OrderViewModel viewModel,
            LabelSource source,
            LabelObjectType objectType,
            LabelState state,
            LabelReasonCodes reason)
        {
            var order = await GetOrder(viewModel.OrderNumber);
            if (order == null)
            {
                return BadRequest("No such order found for this user.");
            }

            #region Fraud Protection Service
            string labelObjectId;
            switch (objectType)
            {
                case LabelObjectType.Purchase:
                    labelObjectId = order.RiskPurchase.PurchaseId;
                    break;
                case LabelObjectType.Account:
                    labelObjectId = order.RiskPurchase.User.UserId;
                    break;
                case LabelObjectType.Email:
                    labelObjectId = order.RiskPurchase.User.Email;
                    break;
                case LabelObjectType.PI:
                    labelObjectId = order.RiskPurchase.PaymentInstrumentList[0].MerchantPaymentInstrumentId;
                    break;
                default:
                    throw new InvalidOperationException("Label object type not supported: " + objectType);
            }

            var label = new Label
            {
                LabelObjectType = objectType.ToString(),
                LabelObjectId = labelObjectId,
                LabelSource = source.ToString(),
                LabelReasonCodes = reason.ToString(),
                LabelState = state.ToString(),
                EventTimeStamp = DateTimeOffset.Now,
                Processor = "Fraud Protection sample site",
                Amount = order.Total,
                Currency = order.RiskPurchase?.Currency,
            };
            var response = await _fraudProtectionService.PostLabel(label);

            var fraudProtectionIO = new FraudProtectionIOModel(label, response, "Label");
            TempData.Put(FraudProtectionIOModel.TempDataKey, fraudProtectionIO);
            #endregion

            return View("FraudLabelDone");
        }

        private async Task<IActionResult> OrderDetailView(string viewName, int orderId)
        {
            var order = await GetOrder(orderId);
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
            var order = await GetOrder(viewModel.OrderNumber);
            if (order == null)
            {
                return BadRequest("No such order found for this user.");
            }

            await modifyOrder(order);

            await _orderRepository.UpdateAsync(order);

            return View(viewName);
        }

        private async Task<Order> GetOrder(int orderId)
        {
            var orders = await GetOrders();
            return orders.FirstOrDefault(o => o.Id == orderId);
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
                    PictureUrl = _uriComposer.ComposePicUri(oi.ItemOrdered.PictureUri),
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
