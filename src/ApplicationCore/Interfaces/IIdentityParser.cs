// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Security.Principal;

namespace Contoso.FraudProtection.ApplicationCore.Interfaces
{
    public interface IIdentityParser<T>
    {
        T Parse(IPrincipal principal);
    }
}
