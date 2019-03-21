// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.OrderAggregate;

namespace Contoso.FraudProtection.ApplicationCore.Interfaces
{
    public interface IOrderRepository : IRepository<Order>, IAsyncRepository<Order>
    {
    }
}
