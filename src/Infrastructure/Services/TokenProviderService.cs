// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Contoso.FraudProtection.ApplicationCore.Interfaces;
using Contoso.FraudProtection.Infrastructure.Utilities;
using System.Threading.Tasks;

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
            var assertionCert = CertificateUtility.GetByThumbprint(_settings.CertificateThumbprint);
            var clientAssertion = new ClientAssertionCertificate(_settings.ClientId, assertionCert);

            var context = new AuthenticationContext(_settings.Authority);
            var authenticationResult = await context.AcquireTokenAsync(resource, clientAssertion);

            return authenticationResult.AccessToken;
        }
    }
}
