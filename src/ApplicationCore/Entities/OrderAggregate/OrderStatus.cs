// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.ComponentModel;

namespace Contoso.FraudProtection.ApplicationCore.Entities.OrderAggregate
{
    public enum OrderStatus: int
    {
        None = 0,
        [Description("Order is received")]
        Received = 1,
        [Description("Order is rejected")]
        Rejected = 2,
        [Description("Order is in progress")]
        InProgress = 3,
        [Description("Order is shipped")]
        Complete = 4,
        [Description("Return initiated by user")]
        ReturnInitiated = 5,
        [Description("Return is rejected")]
        ReturnRejected = 6,
        [Description("Return accepted & is in progress")]
        ReturnInProgress = 7,
        [Description("Return is completed")]
        ReturnCompleted = 8,
        [Description("Charge back")]
        ChargeBack = 9,
        [Description("Order is being reviewed")]
        InReview = 10,
    }
}
