// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Contoso.FraudProtection.ApplicationCore.Services
{
    public static class OrderCalculator
    {
        //Fake amount for sample purposes.
        private const decimal TaxRate = 0.085M;

        public static OrderTotals CalculateTotals(IEnumerable<Tuple<decimal, int>> pricesAndQuantities)
        {
            var totals = new OrderTotals
            {
                SubTotal = Math.Round(
                                pricesAndQuantities
                                    .Select(i => i.Item1 * i.Item2)
                                    .DefaultIfEmpty(0)
                                    .Sum(),
                                2)
            };

            totals.Tax = Math.Round(totals.SubTotal * TaxRate, 2);
            totals.Total = Math.Round(totals.SubTotal + totals.Tax, 2);

            return totals;
        }
    }

    public class OrderTotals
    {
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}
