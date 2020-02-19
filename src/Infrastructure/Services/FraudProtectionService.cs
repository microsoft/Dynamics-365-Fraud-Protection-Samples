// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels;
using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response;
using Contoso.FraudProtection.ApplicationCore.Interfaces;
using Microsoft.Dynamics.FraudProtection.Models;
using Microsoft.Dynamics.FraudProtection.Models.BankEventEvent;
using Microsoft.Dynamics.FraudProtection.Models.ChargebackEvent;
using Microsoft.Dynamics.FraudProtection.Models.LabelEvent;
using Microsoft.Dynamics.FraudProtection.Models.PurchaseEvent;
using Microsoft.Dynamics.FraudProtection.Models.PurchaseStatusEvent;
using Microsoft.Dynamics.FraudProtection.Models.RefundEvent;
using Microsoft.Dynamics.FraudProtection.Models.SignupEvent;
using Microsoft.Dynamics.FraudProtection.Models.SignupStatusEvent;
using Microsoft.Dynamics.FraudProtection.Models.UpdateAccountEvent;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using AccountProtection = Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection;

namespace Contoso.FraudProtection.Infrastructure.Services
{
    #region Fraud Protection Service
    /// <summary>
    /// Service that calls Fraud Protection APIs.
    /// </summary>
    public class FraudProtectionService : IFraudProtectionService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenProvider _tokenProviderService;
        private readonly FraudProtectionSettings _settings;
        private readonly JsonSerializerSettings _serializerSettings;

        public FraudProtectionService(IOptions<FraudProtectionSettings> settings, ITokenProvider tokenProviderService)
        {
            _httpClient = new HttpClient();
            _settings = settings.Value;
            _tokenProviderService = tokenProviderService;
            _serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
        }

        public string NewCorrelationId => Guid.NewGuid().ToString();

        private async Task<HttpResponseMessage> PostAsync<T>(
            string endpoint,
            T content,
            string correlationId)
        {
            var purchaseEventContent = content as IBaseFraudProtectionEvent;
            if (purchaseEventContent != null)
            {
                //All events using the Purchase APIs have the following data
                purchaseEventContent.Metadata = new EventMetadata
                {
                    TrackingId = Guid.NewGuid().ToString(),
                    MerchantTimeStamp = DateTimeOffset.Now
                };
            }

            var authToken = await _tokenProviderService.AcquireTokenAsync();
            var url = $"{_settings.ApiBaseUrl}{endpoint}";
            var serializedObject = JsonConvert.SerializeObject(content, _serializerSettings);
            var serializedContent = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostWithHeadersAsync(
               url,
               serializedContent,
               new Dictionary<string, string>
               {
                    { Constants.CORRELATION_ID, correlationId ?? Guid.NewGuid().ToString() },
                    { Constants.AUTHORIZATION, $"{Constants.BEARER} {authToken}" }
               });

            return response;
        }

        private async Task<T> Read<T>(HttpResponseMessage rawResponse) where T : new()
        {
            rawResponse.EnsureSuccessStatusCode();
            var rawContent = await rawResponse.Content.ReadAsStringAsync();
            rawResponse.Dispose();

            return JsonConvert.DeserializeObject<T>(rawContent);
        }

        public async Task<PurchaseResponse> PostPurchase(Purchase purchase, string correlationId = null)
        {
            var response = await PostAsync(_settings.Endpoints.Purchase, purchase, correlationId);
            return await Read<PurchaseResponse>(response);
        }

        public async Task<FraudProtectionResponse> PostRefund(Refund refund, string correlationId = null)
        {
            var response = await PostAsync(_settings.Endpoints.Refund, refund, correlationId);
            return await Read<FraudProtectionResponse>(response);
        }

        public async Task<FraudProtectionResponse> PostUser(User userAccount, string correlationId = null)
        {
            var response = await PostAsync(_settings.Endpoints.UpdateAccount, userAccount, correlationId);
            return await Read<FraudProtectionResponse>(response);
        }

        public async Task<FraudProtectionResponse> PostBankEvent(BankEvent bankEvent, string correlationId = null)
        {
            var response = await PostAsync(_settings.Endpoints.BankEvent, bankEvent, correlationId);
            return await Read<FraudProtectionResponse>(response);
        }

        public async Task<FraudProtectionResponse> PostChargeback(Chargeback chargeback, string correlationId = null)
        {
            var response = await PostAsync(_settings.Endpoints.Chargeback, chargeback, correlationId);
            return await Read<FraudProtectionResponse>(response);
        }

        public async Task<FraudProtectionResponse> PostPurchaseStatus(PurchaseStatusEvent purchaseStatus, string correlationId = null)
        {
            var response = await PostAsync(_settings.Endpoints.PurchaseStatus, purchaseStatus, correlationId);
            return await Read<FraudProtectionResponse>(response);
        }

        public async Task<SignupResponse> PostSignup(SignUp signup, string correlationId = null)
        {
            var response = await PostAsync(_settings.Endpoints.Signup, signup, correlationId);
            return await Read<SignupResponse>(response);
        }

        public async Task<AccountProtection.Response> PostSignupAP(AccountProtection.SignUp signup, string correlationId = null)
        {
            string endpoint = string.Format(_settings.Endpoints.SignupAP, _settings.AccountProtectionTenantId, signup.Metadata.SignUpId);

            var response = await PostAsync(endpoint, signup, correlationId);
            return await Read<AccountProtection.ResponseSuccess>(response);
        }

        public async Task<FraudProtectionResponse> PostSignupStatus(SignupStatusEvent signupStatus, string correlationId = null)
        {
            var response = await PostAsync(_settings.Endpoints.SignupStatus, signupStatus, correlationId);
            return await Read<FraudProtectionResponse>(response);
        }

        public async Task<FraudProtectionResponse> PostLabel(Label label, string correlationId = null)
        {
            var response = await PostAsync(_settings.Endpoints.Label, label, correlationId);
            return await Read<FraudProtectionResponse>(response);
        }

        public async Task<SignInResponse> PostSignIn(SignIn signIn, string correlationId = null)
        {
            var response = await PostAsync(_settings.Endpoints.SignIn, signIn, correlationId);
            return await Read<SignInResponse>(response);
        }

        public async Task<AccountProtection.Response> PostSignInAP(AccountProtection.SignIn signIn, string correlationId = null)
        {
            string endpoint = string.Format(_settings.Endpoints.SignInAP, _settings.AccountProtectionTenantId, signIn.Metadata.LoginId);

            var response = await PostAsync(endpoint, signIn, correlationId);
            return await Read<AccountProtection.ResponseSuccess>(response);
        }
    }
    #endregion
}
