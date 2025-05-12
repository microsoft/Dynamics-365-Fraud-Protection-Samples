// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Net.Http;

namespace Contoso.FraudProtection.ApplicationCore.Exceptions
{
    public class FraudProtectionApiException : Exception
    {
        public HttpResponseMessage Response { get; }

        public FraudProtectionApiException()
        {
        }

        public FraudProtectionApiException(HttpResponseMessage response) : base($"Fraud Protection API exception occurred!")
        {
            Response = response;
        }

        public FraudProtectionApiException(string message) : base(message)
        {
        }

        public FraudProtectionApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
