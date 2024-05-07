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
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
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

        private async Task<SampleResponse<T>> CallAndRead<T>(Func<Task<HttpResponseMessage>> apiCall) where T : new()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var response = await apiCall();
            watch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                throw new FraudProtectionApiException(response);
            }

            var content = await response.Content.ReadAsStringAsync();
            response.Dispose();

            return new SampleResponse<T>
            {
                Data = JsonSerializer.Deserialize<T>(content, _responseDeserializationOptions),
                RawData = JsonSerializer.Deserialize<object>(content, _responseDeserializationOptions),
                ResponseTime = watch.ElapsedMilliseconds,  
            };
        }

        public async Task<SampleResponse<PurchaseResponse>> PostPurchase(Purchase purchase, string correlationId, string envId)
        {
            return await CallAndRead<PurchaseResponse>(() => PostAsync(_settings.Endpoints.Purchase, purchase, correlationId, envId));
        }

        public async Task<SampleResponse<FraudProtectionResponse>> PostRefund(Refund refund, string correlationId, string envId)
        {
            return await CallAndRead<FraudProtectionResponse>(() => PostAsync(_settings.Endpoints.Refund, refund, correlationId, envId));
        }

        public async Task<SampleResponse<FraudProtectionResponse>> PostUser(User userAccount, string correlationId, string envId)
        {
            return await CallAndRead<FraudProtectionResponse>(() => PostAsync(_settings.Endpoints.UpdateAccount, userAccount, correlationId, envId));
        }

        public async Task<SampleResponse<FraudProtectionResponse>> PostBankEvent(BankEvent bankEvent, string correlationId, string envId)
        {
            return await CallAndRead<FraudProtectionResponse>(() => PostAsync(_settings.Endpoints.BankEvent, bankEvent, correlationId, envId));
        }

        public async Task<SampleResponse<FraudProtectionResponse>> PostChargeback(Chargeback chargeback, string correlationId, string envId)
        {
            return await CallAndRead<FraudProtectionResponse>(() => PostAsync(_settings.Endpoints.Chargeback, chargeback, correlationId, envId));
        }

        public async Task<SampleResponse<FraudProtectionResponse>> PostPurchaseStatus(PurchaseStatusEvent purchaseStatus, string correlationId, string envId)
        {
            return await CallAndRead<FraudProtectionResponse>(() => PostAsync(_settings.Endpoints.PurchaseStatus, purchaseStatus, correlationId, envId));
        }

        public async Task<SampleResponse<Response>> PostSignup(AccountProtection.SignUp signup, string correlationId, string envId)
        {
            var endpoint = string.Format(_settings.Endpoints.SignupAP, signup.Metadata.SignUpId);
            return await CallAndRead<Response>(() => PostAsync(endpoint, signup, correlationId, envId));
        }

        public async Task<SampleResponse<FraudProtectionResponse>> PostSignupStatus(SignupStatusEvent signupStatus, string correlationId, string envId)
        {
            return await CallAndRead<FraudProtectionResponse>(() => PostAsync(_settings.Endpoints.SignupStatus, signupStatus, correlationId, envId));
        }

        public async Task<SampleResponse<FraudProtectionResponse>> PostLabel(Label label, string correlationId, string envId)
        {
            return await CallAndRead<FraudProtectionResponse>(() => PostAsync(_settings.Endpoints.Label, label, correlationId, envId));
        }

        public async Task<SampleResponse<Response>> PostSignIn(AccountProtection.SignIn signIn, string correlationId, string envId)
        {
            var endpoint = string.Format(_settings.Endpoints.SignInAP, signIn.Metadata.LoginId);

            return await CallAndRead<Response>(() => PostAsync(endpoint, signIn, correlationId, envId));
        }

        public async Task<SampleResponse<Response>> PostCustomAssessment(CustomAssessment assessment, string correlationId, string envId)
        {
            var endpoint = string.Format(_settings.Endpoints.CustomAssessment, assessment.ApiName);

            return await CallAndRead<Response>(() => PostAsync(endpoint, assessment.Payload, correlationId, envId, true));
        }

        public async Task<SampleResponse<AssessmentResponse>> PostAssessment(CustomAssessment assessment, string correlationId, string envId)
        {
            var endpoint = string.Format(_settings.Endpoints.Assessment, assessment.ApiName);
            return await CallAndRead<AssessmentResponse>(() => PostAsync(endpoint, assessment.Payload, correlationId, envId, true));
        }
    }
    #endregion
}
