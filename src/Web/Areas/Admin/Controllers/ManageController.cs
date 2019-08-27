// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.OrderAggregate;
using Contoso.FraudProtection.ApplicationCore.Interfaces;
using Contoso.FraudProtection.Infrastructure.Data;
using Contoso.FraudProtection.Web.Extensions;
using Contoso.FraudProtection.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Dynamics.FraudProtection.Models.RefundEvent;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Manager")]
    public class ManageController : Controller
    {
        private readonly CatalogContext _context;
        private readonly IFraudProtectionService _fraudProtectionService;

        public ManageController(CatalogContext context, IFraudProtectionService fraudProtectionService)
        {
            _context = context;
            _fraudProtectionService = fraudProtectionService;
        }

        // GET: Admin/Manage
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.ToListAsync());
        }

        public async Task<IActionResult> Types()
        {
            return View(await _context.CatalogTypes.ToListAsync());
        }

        public async Task<IActionResult> Brands()
        {
            return View(await _context.CatalogBrands.ToListAsync());
        }

        public async Task<IActionResult> Catalogs()
        {
            return View(await _context.CatalogItems.ToListAsync());
        }

        // GET: Admin/Manage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(o=>o.OrderItems)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Admin/Manage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(o => o.OrderItems)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            ViewData["SelectList"] = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>()
                .Select(s => new SelectListItem(s.GetDescription(), ((int)s).ToString(), order.Status == s));
            return View(order);
        }

        // POST: Admin/Manage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            var dbOrder = await _context
                .Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (id != order.Id && dbOrder == null)
            {
                return NotFound();
            }

            dbOrder.AdminComments = order.AdminComments;
            dbOrder.Status = order.Status;

            if (ModelState.IsValid)
            {
                try
                {
                    if (dbOrder.RiskRefundResponse == null && (order.Status == OrderStatus.ReturnInProgress || order.Status == OrderStatus.ReturnCompleted))
                    {
                        var refund = new Refund
                        {
                            RefundId = Guid.NewGuid().ToString(),
                            Amount = dbOrder.Total,
                            Currency = dbOrder.RiskPurchase?.Currency,
                            BankEventTimestamp = DateTimeOffset.Now,
                            Purchase = new RefundPurchase { PurchaseId = dbOrder.RiskPurchase.PurchaseId },
                            Reason = order.ReturnOrChargebackReason,
                            Status = order.Status == OrderStatus.ReturnInProgress ? RefundStatus.INITIATED.ToString(): RefundStatus.COMPLETED.ToString(),
                            User = new RefundUser { UserId = dbOrder.RiskPurchase?.User?.UserId },
                        };

                        var riskResponse = await _fraudProtectionService.PostRefund(refund);
                        dbOrder.RiskRefund = refund;
                        dbOrder.RiskRefundResponse = riskResponse;

                        var fraudProtectionIO = new FraudProtectionIOModel(refund, riskResponse, "Refund");
                        TempData.Put(FraudProtectionIOModel.TempDataKey, fraudProtectionIO);
                    }

                    _context.Update(dbOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/Admin/Manage/Index");
            }
            return View(dbOrder);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
