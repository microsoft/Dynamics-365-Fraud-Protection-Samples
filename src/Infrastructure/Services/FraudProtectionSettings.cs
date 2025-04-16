// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Security.Cryptography.X509Certificates;

namespace Contoso.FraudProtection.Infrastructure.Services
{
    #region Fraud Protection Service
    public class FraudProtectionSettings
    {
        public string InstanceId { get; set; }
        public string ApiBaseUrl { get; set; }
        public FraudProtectionEndpoints Endpoints { get; set; }
    }

    public class FraudProtectionEndpoints
    {
        public string Purchase { get; set; }
        public string PurchaseStatus { get; set; }
        public string BankEvent { get; set; }
        public string Chargeback { get; set; }
        public string Label { get; set; }
        public string Refund { get; set; }
        public string Signup { get; set; }
        public string SignupAP { get; set; }
        public string SignupStatus { get; set; }
        public string UpdateAccount { get; set; }
        public string SignIn { get; set; }
        public string SignInAP { get; set; }
        public string CustomAssessment { get; set; }
        public string Assessment { get; set; }
    }

    public class TokenProviderServiceSettings
    {
        public string Resource { get; set; }
        public string ClientId { get; set; }
        public string Authority { get; set; }
        public string ClientSecret { get; set; }
        public string CertificateThumbprint { get; set; }
        public StoreLocation CertificateLocation { get; set; }
        public bool? UseSNI { get; set; }
    }
    #endregion
}
