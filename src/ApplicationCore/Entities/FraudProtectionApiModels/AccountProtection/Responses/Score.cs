using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class Score
    {
        public string ScoreType { get; set; }

        public double ScoreValue { get; set; }

        public string Reason { get; set; }
    }
}
