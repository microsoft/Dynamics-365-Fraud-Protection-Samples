using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.AccountProtection
{
    public class ResponseError : Response
    {
        public bool IsRetryable { get; set; }

        public string Message { get; set; }

        public List<ResponseError> OtherErrors { get; set; }
    }
}
