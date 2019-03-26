// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Security.Cryptography.X509Certificates;

namespace Contoso.FraudProtection.Infrastructure.Utilities
{
    public static class CertificateUtility
    {
        public static X509Certificate2 GetCertificateBy<T>(X509FindType findType, T findValue, bool findValidOnly = true)
        {
            var certStore = new X509Store(StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certs = certStore.Certificates.Find(findType, findValue, findValidOnly);
            X509Certificate2 cert = certs.Count > 0 ? certs[0] : null;

            return cert;
        }

        public static X509Certificate2 GetCertificateByName(string certName)
        {
            return GetCertificateBy(X509FindType.FindBySubjectDistinguishedName, certName);
        }

        public static X509Certificate2 GetByThumbprint(string thumbprint)
        {
            return GetCertificateBy(X509FindType.FindByThumbprint, thumbprint);
        }
    }
}
