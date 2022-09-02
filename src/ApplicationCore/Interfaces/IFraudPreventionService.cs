// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels;
using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection.Response;
using Microsoft.Dynamics.FraudProtection.Models.BankEventEvent;
using Microsoft.Dynamics.FraudProtection.Models.ChargebackEvent;
using Microsoft.Dynamics.FraudProtection.Models.LabelEvent;
using Microsoft.Dynamics.FraudProtection.Models.PurchaseEvent;
using Microsoft.Dynamics.FraudProtection.Models.PurchaseStatusEvent;
using Microsoft.Dynamics.FraudProtection.Models.RefundEvent;
using Microsoft.Dynamics.FraudProtection.Models.SignupStatusEvent;
using Microsoft.Dynamics.FraudProtection.Models.UpdateAccountEvent;
using System.Threading.Tasks;

using AccountProtection = Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection;

namespace Contoso.FraudProtection.ApplicationCore.Interfaces
{
    #region Fraud Protection Service
    public interface IFraudProtectionService
    {
        string NewCorrelationId { get; }

        Task<PurchaseResponse> PostPurchase(Purchase purchase, string correlationId, string envId);

        Task<Response> PostSignup(AccountProtection.SignUp signup, string correlationId, string envId);

        Task<FraudProtectionResponse> PostRefund(Refund refund, string correlationId, string envId);

        Task<FraudProtectionResponse> PostUser(User userAccount, string correlationId, string envId);

        Task<FraudProtectionResponse> PostBankEvent(BankEvent bankEvent, string correlationId, string envId);

        Task<FraudProtectionResponse> PostChargeback(Chargeback chargeback, string correlationId, string envId);

        Task<FraudProtectionResponse> PostPurchaseStatus(PurchaseStatusEvent purchaseStatus, string correlationId, string envId);

        Task<FraudProtectionResponse> PostSignupStatus(SignupStatusEvent signupStatus, string correlationId, string envId);

        Task<FraudProtectionResponse> PostLabel(Label label, string correlationId, string envId);

        Task<Response> PostSignIn(AccountProtection.SignIn request, string correlationId, string envId);

        Task<Response> PostCustomAssessment(CustomAssessment assessment, string correlationId, string envId, bool useV2);
    }

    #endregion
}
