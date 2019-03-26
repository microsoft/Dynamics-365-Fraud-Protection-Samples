// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Threading.Tasks;

namespace Contoso.FraudProtection.ApplicationCore.Interfaces
{
    public interface ITokenProvider
    {
        Task<string> AcquireTokenAsync(string resource);
    }
}
