// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels;
using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection.Response;
using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response;
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

        Task<SampleResponse<PurchaseResponse>> PostPurchase(Purchase purchase, string correlationId, string envId);

        Task<SampleResponse<FraudProtectionResponse>> PostRefund(Refund refund, string correlationId, string envId);

        Task<SampleResponse<FraudProtectionResponse>> PostUser(User userAccount, string correlationId, string envId);

        Task<SampleResponse<FraudProtectionResponse>> PostBankEvent(BankEvent bankEvent, string correlationId, string envId);

        Task<SampleResponse<FraudProtectionResponse>> PostChargeback(Chargeback chargeback, string correlationId, string envId);

        Task<SampleResponse<FraudProtectionResponse>> PostPurchaseStatus(PurchaseStatusEvent purchaseStatus, string correlationId, string envId);

        Task<SampleResponse<FraudProtectionResponse>> PostSignupStatus(SignupStatusEvent signupStatus, string correlationId, string envId);

        Task<SampleResponse<FraudProtectionResponse>> PostLabel(Label label, string correlationId, string envId);

        Task<SampleResponse<Response>> PostSignup(AccountProtection.SignUp signup, string correlationId, string envId);

        Task<SampleResponse<Response>> PostSignIn(AccountProtection.SignIn request, string correlationId, string envId);

        Task<SampleResponse<Response>> PostCustomAssessment(CustomAssessment assessment, string correlationId, string envId);

        Task<SampleResponse<object>> PostAssessment(CustomAssessment assessment, string correlationId, string envId);
    }

    #endregion
}
