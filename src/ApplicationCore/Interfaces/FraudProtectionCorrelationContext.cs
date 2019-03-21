// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Contoso.FraudProtection.Infrastructure.Services
{
    #region Fraud Protection Service
    public class FraudProtectionCorrelationContext
    {
        public FraudProtectionCorrelationContext(string correlationId)
        {
            CorrelationId = correlationId;
        }
        
        public string CorrelationId { get; }
	}
    #endregion
}
