// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels;
using Contoso.FraudProtection.ApplicationCore.Interfaces;
using Microsoft.Dynamics.FraudProtection.Models.BankEventEvent;
using Microsoft.Dynamics.FraudProtection.Models.ChargebackEvent;
using Microsoft.Dynamics.FraudProtection.Models.PurchaseEvent;
using Microsoft.Dynamics.FraudProtection.Models.PurchaseStatusEvent;
using Microsoft.Dynamics.FraudProtection.Models.RefundEvent;
using Microsoft.Dynamics.FraudProtection.Models.SignupEvent;
using Microsoft.Dynamics.FraudProtection.Models.SignupStatusEvent;
using Microsoft.Dynamics.FraudProtection.Models.UpdateAccountEvent;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
            _serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        }

        public string NewCorrelationId => Guid.NewGuid().ToString();

        private async Task<HttpResponseMessage> PostAsync<T>(
            string endpoint, 
            T content,
            string correlationId,
            Action<Dictionary<string, object>> modifyBody = null)
        {
            //All events have the following format
            var contentWithTimestamp = new Dictionary<string, object>
            {
                { "MerchantLocalDate", DateTimeOffset.Now },
                { "Data", content },
            };

            modifyBody?.Invoke(contentWithTimestamp);

            var authToken = await _tokenProviderService.AcquireTokenAsync();
            var url = $"{_settings.ApiBaseUrl}{endpoint}";
            var serializedObject = JsonConvert.SerializeObject(contentWithTimestamp, _serializerSettings);
            var serializedContent = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostWithHeadersAsync(
               url,
               serializedContent,
               new Dictionary<string, string>
               {
                    { Constants.CORRELATION_ID, correlationId ?? Guid.NewGuid().ToString() },
                    { Constants.TRACKING_ID, Guid.NewGuid().ToString() },
                    { Constants.AUTHORIZATION, $"{Constants.BEARER} {authToken}" }
               });

            return response;
        }

        private static async Task<T> Read<T>(HttpResponseMessage rawResponse) where T : BaseResponse, new()
        {
            rawResponse.EnsureSuccessStatusCode();
            var rawContent = await rawResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<T>(rawContent);
            response.StatusCode = rawResponse.StatusCode;
            response.IsSuccessStatusCode = rawResponse.IsSuccessStatusCode;
            
            return response;
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
            //UpdateAccount is the only endpoint that needs 'CustomerLocalDate'
            var response = await PostAsync(
                _settings.Endpoints.UpdateAccount,
                userAccount,
                correlationId,
                body => body["CustomerLocalDate"] = DateTimeOffset.Now);

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

        public async Task<FraudProtectionResponse> PostSignupStatus(SignupStatusEvent signupStatus, string correlationId = null)
        {
            var response = await PostAsync(_settings.Endpoints.SignupStatus, signupStatus, correlationId);
            return await Read<FraudProtectionResponse>(response);
        }
    }
    #endregion
}
