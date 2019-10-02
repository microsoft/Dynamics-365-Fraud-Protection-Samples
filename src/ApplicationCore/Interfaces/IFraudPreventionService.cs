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

namespace Contoso.FraudProtection.ApplicationCore.Interfaces
{
    #region Fraud Protection Service
    public interface IFraudProtectionService
    {
        string NewCorrelationId { get; }

        Task<PurchaseResponse> PostPurchase(Purchase purchase, string correlationId = null);

        Task<SignupResponse> PostSignup(SignUp signup, string correlationId = null);

        Task<FraudProtectionResponse> PostRefund(Refund refund, string correlationId = null);

        Task<FraudProtectionResponse> PostUser(User userAccount, string correlationId = null);

        Task<FraudProtectionResponse> PostBankEvent(BankEvent bankEvent, string correlationId = null);

        Task<FraudProtectionResponse> PostChargeback(Chargeback chargeback, string correlationId = null);

        Task<FraudProtectionResponse> PostPurchaseStatus(PurchaseStatusEvent purchaseStatus, string correlationId = null);

        Task<FraudProtectionResponse> PostSignupStatus(SignupStatusEvent signupStatus, string correlationId = null);

        Task<FraudProtectionResponse> PostLabel(Label label, string correlationId = null);

        Task<SignInResponse> PostSignIn(SignIn request, string correlationId = null);
    }

    #endregion
}
