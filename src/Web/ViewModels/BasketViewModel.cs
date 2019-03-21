// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Services;
using System.Collections.Generic;

namespace Contoso.FraudProtection.Web.ViewModels
{
    public class BasketViewModel : OrderTotals
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }

        public List<BasketItemViewModel> Items { get; set; } = new List<BasketItemViewModel>();
    }
}
