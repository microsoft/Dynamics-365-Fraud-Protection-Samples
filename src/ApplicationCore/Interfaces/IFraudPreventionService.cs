// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels;
using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response;
using Microsoft.Dynamics.FraudProtection.Models.BankEventEvent;
using Microsoft.Dynamics.FraudProtection.Models.ChargebackEvent;
using Microsoft.Dynamics.FraudProtection.Models.LabelEvent;
using Microsoft.Dynamics.FraudProtection.Models.PurchaseEvent;
using Microsoft.Dynamics.FraudProtection.Models.PurchaseStatusEvent;
using Microsoft.Dynamics.FraudProtection.Models.RefundEvent;
using Microsoft.Dynamics.FraudProtection.Models.SignupEvent;
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

        Task<PurchaseResponse> PostPurchase(Purchase purchase, string correlationId);

        Task<SignupResponse> PostSignup(SignUp signup, string correlationId);

        Task<AccountProtection.Response> PostSignupAP(AccountProtection.SignUp signup, string correlationId);

        Task<FraudProtectionResponse> PostRefund(Refund refund, string correlationId);

        Task<FraudProtectionResponse> PostUser(User userAccount, string correlationId);

        Task<FraudProtectionResponse> PostBankEvent(BankEvent bankEvent, string correlationId);

        Task<FraudProtectionResponse> PostChargeback(Chargeback chargeback, string correlationId);

        Task<FraudProtectionResponse> PostPurchaseStatus(PurchaseStatusEvent purchaseStatus, string correlationId);

        Task<FraudProtectionResponse> PostSignupStatus(SignupStatusEvent signupStatus, string correlationId);

        Task<FraudProtectionResponse> PostLabel(Label label, string correlationId);

        Task<SignInResponse> PostSignIn(SignIn request, string correlationId);

        Task<AccountProtection.Response> PostSignInAP(AccountProtection.SignIn request, string correlationId);
        
        Task<AccountProtection.Response> PostCustomAssessment(CustomAssessment assessment, string correlationId);
    }

    #endregion
}
