using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class ResultDetail
    {
        public string Decision { get; set; }

        public string ChallengeType { get; set; }

        public string[] Reasons { get; set; }

        public string RuleSetId { get; set; }

        public string[] SupportingMessages { get; set; }

        public Dictionary<string, object> Other { get; set; }

        public List<Score> Scores { get; set; }
    }
}
