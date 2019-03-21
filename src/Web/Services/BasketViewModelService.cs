// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Interfaces;
using Contoso.FraudProtection.ApplicationCore.Specifications;
using Contoso.FraudProtection.ApplicationCore.Entities;
using Contoso.FraudProtection.ApplicationCore.Entities.BasketAggregate;
using Contoso.FraudProtection.Web.Interfaces;
using Contoso.FraudProtection.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Contoso.FraudProtection.ApplicationCore.Services;

namespace Contoso.FraudProtection.Web.Services
{
    public class BasketViewModelService : IBasketViewModelService
    {
        private readonly IAsyncRepository<Basket> _basketRepository;
        private readonly IUriComposer _uriComposer;
        private readonly IRepository<CatalogItem> _itemRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;

        public BasketViewModelService(IAsyncRepository<Basket> basketRepository,
            IRepository<CatalogItem> itemRepository,
            IUriComposer uriComposer,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _uriComposer = uriComposer;
            _itemRepository = itemRepository;
            _contextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<BasketViewModel> GetOrCreateBasketForUser(string userName)
        {
            var basketSpec = new BasketWithItemsSpecification(userName);
            var basket = (await _basketRepository.ListAsync(basketSpec)).FirstOrDefault();

            if (basket == null)
            {
                return await CreateBasketForUser(userName);
            }
            return CreateViewModelFromBasket(basket);
        }

        private BasketViewModel CreateViewModelFromBasket(Basket basket)
        {
            var pricesAndQuantities = basket.Items.Select(i => new System.Tuple<decimal, int>(i.UnitPrice, i.Quantity));
            var totals = OrderCalculator.CalculateTotals(pricesAndQuantities);

            return new BasketViewModel
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                SubTotal = totals.SubTotal,
                Tax = totals.Tax,
                Total = totals.Total,
                Items = basket.Items.Select(i =>
                {
                    var itemModel = new BasketItemViewModel
                    {
                        Id = i.Id,
                        UnitPrice = i.UnitPrice,
                        Quantity = i.Quantity,
                        CatalogItemId = i.CatalogItemId

                    };
                    var item = _itemRepository.GetById(i.CatalogItemId);
                    itemModel.PictureUrl = _uriComposer.ComposePicUri(item.PictureUri);
                    itemModel.ProductName = item.Name;
                    return itemModel;
                })
                .ToList()
            };
        }

        private async Task<BasketViewModel> CreateBasketForUser(string userId)
        {
            var basket = new Basket { BuyerId = userId };
            await _basketRepository.AddAsync(basket);

            return new BasketViewModel
            {
                BuyerId = basket.BuyerId,
                Id = basket.Id,
                Items = new List<BasketItemViewModel>()
            };
        }
    }
}
