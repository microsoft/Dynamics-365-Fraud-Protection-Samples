// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels;
using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection.Response;
using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response;
using Contoso.FraudProtection.ApplicationCore.Exceptions;
using Contoso.FraudProtection.ApplicationCore.Interfaces;
using Microsoft.Dynamics.FraudProtection.Models;
using Microsoft.Dynamics.FraudProtection.Models.BankEventEvent;
using Microsoft.Dynamics.FraudProtection.Models.ChargebackEvent;
using Microsoft.Dynamics.FraudProtection.Models.LabelEvent;
using Microsoft.Dynamics.FraudProtection.Models.PurchaseEvent;
using Microsoft.Dynamics.FraudProtection.Models.PurchaseStatusEvent;
using Microsoft.Dynamics.FraudProtection.Models.RefundEvent;
using Microsoft.Dynamics.FraudProtection.Models.SignupStatusEvent;
using Microsoft.Dynamics.FraudProtection.Models.UpdateAccountEvent;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
        private readonly JsonSerializerOptions _requestSerializationOptions;
        private readonly JsonSerializerOptions _responseDeserializationOptions;

        public FraudProtectionService(IOptions<FraudProtectionSettings> settings, ITokenProvider tokenProviderService)
        {
            _httpClient = new HttpClient();
            _settings = settings.Value;
            _tokenProviderService = tokenProviderService;
            _requestSerializationOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _responseDeserializationOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public string NewCorrelationId => Guid.NewGuid().ToString();

        private async Task<HttpResponseMessage> PostAsync<T>(
            string endpoint,
            T content,
            string correlationId,
            string envId = null,
            bool skipSerialization = false)
        {
            if (content is IBaseFraudProtectionEvent purchaseEventContent)
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
            var serializedObject = skipSerialization ? content as string : JsonSerializer.Serialize(content, _requestSerializationOptions);
            var serializedContent = new StringContent(serializedObject, Encoding.UTF8, "application/json");

            return await _httpClient.PostWithHeadersAsync(
               url,
               serializedContent,
               new Dictionary<string, string>
               {
                    { Constants.CORRELATION_ID, correlationId },
                    { Constants.AUTHORIZATION, $"{Constants.BEARER} {authToken}" },
                    { Constants.ENVIRONMENT_ID,  envId ?? _settings.InstanceId },
               });
        }

        private async Task<T> Read<T>(HttpResponseMessage response) where T : new()
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new FraudProtectionApiException(response);
            }

            var content = await response.Content.ReadAsStringAsync();
            response.Dispose();

            return JsonSerializer.Deserialize<T>(content, _responseDeserializationOptions);
        }

        public async Task<PurchaseResponse> PostPurchase(Purchase purchase, string correlationId, string envId)
        {
            var response = await PostAsync(_settings.Endpoints.Purchase, purchase, correlationId, envId);
            return await Read<PurchaseResponse>(response);
        }

        public async Task<FraudProtectionResponse> PostRefund(Refund refund, string correlationId, string envId)
        {
            var response = await PostAsync(_settings.Endpoints.Refund, refund, correlationId, envId);
            return await Read<FraudProtectionResponse>(response);
        }

        public async Task<FraudProtectionResponse> PostUser(User userAccount, string correlationId, string envId)
        {
            var response = await PostAsync(_settings.Endpoints.UpdateAccount, userAccount, correlationId, envId);
            return await Read<FraudProtectionResponse>(response);
        }

        public async Task<FraudProtectionResponse> PostBankEvent(BankEvent bankEvent, string correlationId, string envId)
        {
            var response = await PostAsync(_settings.Endpoints.BankEvent, bankEvent, correlationId, envId);
            return await Read<FraudProtectionResponse>(response);
        }

        public async Task<FraudProtectionResponse> PostChargeback(Chargeback chargeback, string correlationId, string envId)
        {
            var response = await PostAsync(_settings.Endpoints.Chargeback, chargeback, correlationId, envId);
            return await Read<FraudProtectionResponse>(response);
        }

        public async Task<FraudProtectionResponse> PostPurchaseStatus(PurchaseStatusEvent purchaseStatus, string correlationId, string envId)
        {
            var response = await PostAsync(_settings.Endpoints.PurchaseStatus, purchaseStatus, correlationId, envId);
            return await Read<FraudProtectionResponse>(response);
        }

        public async Task<Response> PostSignup(AccountProtection.SignUp signup, string correlationId, string envId)
        {
            string endpoint = string.Format(_settings.Endpoints.SignupAP, signup.Metadata.SignUpId);

            var response = await PostAsync(endpoint, signup, correlationId, envId);
            return await Read<ResponseSuccess>(response);
        }

        public async Task<FraudProtectionResponse> PostSignupStatus(SignupStatusEvent signupStatus, string correlationId, string envId)
        {
            var response = await PostAsync(_settings.Endpoints.SignupStatus, signupStatus, correlationId, envId);
            return await Read<FraudProtectionResponse>(response);
        }

        public async Task<FraudProtectionResponse> PostLabel(Label label, string correlationId, string envId)
        {
            var response = await PostAsync(_settings.Endpoints.Label, label, correlationId, envId);
            return await Read<FraudProtectionResponse>(response);
        }

        public async Task<Response> PostSignIn(AccountProtection.SignIn signIn, string correlationId, string envId)
        {
            string endpoint = string.Format(_settings.Endpoints.SignInAP, signIn.Metadata.LoginId);

            var response = await PostAsync(endpoint, signIn, correlationId, envId);
            return await Read<ResponseSuccess>(response);
        }

        public async Task<Response> PostCustomAssessment(CustomAssessment assessment, string correlationId, string envId)
        {
            string endpoint = string.Format(_settings.Endpoints.CustomAssessment, assessment.ApiName);

            var response = await PostAsync(endpoint, assessment.Payload, correlationId, envId, true);
            return await Read<ResponseSuccess>(response);
        }

        public async Task<AssessmentResponse> PostAssessment(CustomAssessment assessment, string correlationId, string envId)
        {
            string endpoint = string.Format(_settings.Endpoints.Assessment, assessment.ApiName);

            var response = await PostAsync(endpoint, assessment.Payload, correlationId, envId, true);
            return await Read<AssessmentResponse>(response);
        }
    }
    #endregion
}
