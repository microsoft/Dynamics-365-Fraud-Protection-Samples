// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Contoso.FraudProtection.ApplicationCore.Interfaces;
using Contoso.FraudProtection.Infrastructure.Utilities;
using System.Threading.Tasks;
using System;

namespace Contoso.FraudProtection.Infrastructure.Services
{
    public class TokenProviderService : ITokenProvider
    {
        private readonly TokenProviderServiceSettings _settings;

        public TokenProviderService(IOptions<TokenProviderServiceSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<string> AcquireTokenAsync(string resource)
        {
            if (string.IsNullOrEmpty(_settings.CertificateThumbprint) && string.IsNullOrEmpty(_settings.ClientSecret))
                throw new InvalidOperationException("Configure the token provider settings in the appsettings.json file.");

            if (_settings.CertificateThumbprint != "" && _settings.ClientSecret != "")
                throw new InvalidOperationException("Only configure certificate or secret authenticate, not both, in the appsettings file.");

            return _settings.CertificateThumbprint != "" ?
                await AcquireTokenWithCertificateAsync(resource) :
                await AcquireTokenWithSecretAsync(resource);
        }

        private async Task<string> AcquireTokenWithCertificateAsync(string resource)
        {
            var x509Cert = CertificateUtility.GetByThumbprint(_settings.CertificateThumbprint);
            var clientAssertion = new ClientAssertionCertificate(_settings.ClientId, x509Cert);
            var context = new AuthenticationContext(_settings.Authority);
            var authenticationResult = await context.AcquireTokenAsync(resource, clientAssertion);

            return authenticationResult.AccessToken;
        }

        private async Task<string> AcquireTokenWithSecretAsync(string resource)
        {
            var clientAssertion = new ClientCredential(_settings.ClientId, _settings.ClientSecret);
            var context = new AuthenticationContext(_settings.Authority);
            var authenticationResult = await context.AcquireTokenAsync(resource, clientAssertion);

            return authenticationResult.AccessToken;
        }
    }
}
