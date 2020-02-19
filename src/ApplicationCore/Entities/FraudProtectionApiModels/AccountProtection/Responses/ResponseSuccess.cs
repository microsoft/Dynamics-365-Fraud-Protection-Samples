using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class ResponseSuccess : Response
    {
        public List<ResultDetail> ResultDetails { get; set; }
    }
}
