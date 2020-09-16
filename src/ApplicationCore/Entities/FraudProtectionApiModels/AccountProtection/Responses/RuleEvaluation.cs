// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.


using System.Collections.Generic;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection.Responses
{
    public class RuleEvaluation
    {
        public string RuleSetName { get; set; }

        public IEnumerable<string> ClauseNames { get; set; }
    }
}
