// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Interfaces;
using Contoso.FraudProtection.Infrastructure.Utilities;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;

namespace Contoso.FraudProtection.Infrastructure.Services
{
    public class TokenProviderService : ITokenProvider
    {
        private readonly IConfidentialClientApplication _tokenApp;
        private readonly string[] _scopes;

        public TokenProviderService(IOptions<TokenProviderServiceSettings> settingsOption)
        {
            var settings = settingsOption.Value;

            _scopes = new[] { settings.Resource + "/.default" };

            if (string.IsNullOrEmpty(settings.CertificateThumbprint) && string.IsNullOrEmpty(settings.ClientSecret))
                throw new InvalidOperationException("Configure the token provider settings in the appsettings.json file.");

            if (settings.CertificateThumbprint != "" && settings.ClientSecret != "")
                throw new InvalidOperationException("Only configure certificate or secret authenticate, not both, in the appsettings file.");

            var builder = ConfidentialClientApplicationBuilder
                .Create(settings.ClientId)
                .WithAuthority(new Uri(settings.Authority));

            if (settings.CertificateThumbprint != "")
            {
                var x509Cert = CertificateUtility.GetByThumbprint(settings.CertificateThumbprint);
                builder = builder.WithCertificate(x509Cert);
            }
            else
            {
                builder = builder.WithClientSecret(settings.ClientSecret);
            }

            _tokenApp = builder.Build();
        }

        public async Task<string> AcquireTokenAsync()
        {
            var result = await _tokenApp.AcquireTokenForClient(_scopes).ExecuteAsync();
            return result.AccessToken;
        }
    }
}
