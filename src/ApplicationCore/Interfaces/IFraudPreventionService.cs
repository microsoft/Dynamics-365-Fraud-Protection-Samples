// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Dynamics.FraudProtection.Models.BankEventEvent;
using Microsoft.Dynamics.FraudProtection.Models.ChargebackEvent;
using Microsoft.Dynamics.FraudProtection.Models.RefundEvent;
using Microsoft.Dynamics.FraudProtection.Models.UpdateAccountEvent;
using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels;
using System.Threading.Tasks;
using PurchaseEvent = Microsoft.Dynamics.FraudProtection.Models.PurchaseEvent.Purchase;
using PurchaseStatusEvent = Microsoft.Dynamics.FraudProtection.Models.PurchaseStatusEvent.Purchase;

namespace Contoso.FraudProtection.ApplicationCore.Interfaces
{
    #region Fraud Protection Service
    public interface IFraudProtectionService
    {
        string NewCorrelationId { get; }

        Task<PurchaseResponse> PostPurchase(PurchaseEvent purchase, string correlationId = null);

        Task<FraudProtectionResponse> PostRefund(Refund refund, string correlationId = null);

        Task<FraudProtectionResponse> PostUser(User userAccount, string correlationId = null);

        Task<FraudProtectionResponse> PostBankEvent(BankEvent bankEvent, string correlationId = null);

        Task<FraudProtectionResponse> PostChargeback(Chargeback chargeback, string correlationId = null);

        Task<FraudProtectionResponse> PostPurchaseStatus(PurchaseStatusEvent purchaseStatus, string correlationId = null);
    }
    #endregion
}
