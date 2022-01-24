// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.


using System.Collections.Generic;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response
{
    public class RuleEvaluation
    {
        public string Rule { get; set; }

        public IEnumerable<string> ClauseNames { get; set; }

        public string EnvironmentId { get; set; }
    }
}
