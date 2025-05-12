// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Security.Cryptography.X509Certificates;

namespace Contoso.FraudProtection.Infrastructure.Utilities
{
    public static class CertificateUtility
    {
        private static X509Certificate2 GetCertificateBy<T>(X509FindType findType, T findValue, StoreLocation location)
        {
            var certStore = new X509Store(location);
            certStore.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certs = certStore.Certificates.Find(findType, findValue, false);
            if(certs.Count > 0)
            {
                return certs[0];
            }
            else
            {
                throw new Exception("Certificate is missing from your current user storage.");
            }
        }

        public static X509Certificate2 GetByThumbprint(string thumbprint, StoreLocation location)
        {
            return GetCertificateBy(X509FindType.FindByThumbprint, thumbprint, location);
        }
    }
}
