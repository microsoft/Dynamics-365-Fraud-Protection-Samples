// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class Score
    {
        public string ScoreType { get; set; }

        public double ScoreValue { get; set; }

        public string Reason { get; set; }
    }
}
