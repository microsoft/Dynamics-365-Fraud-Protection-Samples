// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Azure.Core;
using Contoso.FraudProtection.ApplicationCore.Interfaces;
using Contoso.FraudProtection.Infrastructure.Utilities;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Contoso.FraudProtection.Infrastructure.Services
{
    public class TokenProviderService : ITokenProvider
    {
        private readonly IConfidentialClientApplication _tokenApp;
        private readonly string[] _scopes;
        private readonly TokenCredential _credential;

        public TokenProviderService(IOptions<TokenProviderServiceSettings> settingsOption, TokenCredential credential)
        {
            var settings = settingsOption.Value;

            _scopes = [settings.Resource + "/.default"];

            if (string.IsNullOrEmpty(settings.CertificateThumbprint) && string.IsNullOrEmpty(settings.ClientSecret) && credential == null)
                throw new InvalidOperationException("Configure the token provider settings in the appsettings.json file or ensure a managed ID credential is available.");

            if (settings.EnableManagedIdentity.GetValueOrDefault())
            { 
                _credential = credential;
                return;
            }

            if (settings.CertificateThumbprint != "" && settings.ClientSecret != "")
                throw new InvalidOperationException("Only configure certificate or secret authenticate, not both, in the appsettings file.");

            var builder = ConfidentialClientApplicationBuilder
                .Create(settings.ClientId)
                .WithAuthority(new Uri(settings.Authority));

            if (settings.CertificateThumbprint != "")
            {
                var x509Cert = CertificateUtility.GetByThumbprint(settings.CertificateThumbprint, settings.CertificateLocation.Value);
                builder = builder.WithCertificate(x509Cert, settings.UseSNI ?? false);
            }
            else if (settings.ClientSecret != "")
            {
                builder = builder.WithClientSecret(settings.ClientSecret);
            }

            _tokenApp = builder.Build();
        }

        public async Task<string> AcquireTokenAsync()
        {
            if (_credential != null)
            {
                var miToken = await _credential.GetTokenAsync(new TokenRequestContext(_scopes), new CancellationToken());
                return miToken.Token;
            }

            var result = await _tokenApp.AcquireTokenForClient(_scopes).ExecuteAsync();
            return result.AccessToken;
        }
    }
}
