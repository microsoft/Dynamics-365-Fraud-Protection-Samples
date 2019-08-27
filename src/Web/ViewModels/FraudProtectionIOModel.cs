// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Contoso.FraudProtection.Web.ViewModels
{
    public class FraudProtectionIOModel
    {
        public const string TempDataKey = "FraudProtectionIOData";

        public List<(string Request, string Response, string Name)> RequestResponsePairs = new List<(string Request, string Response, string Name)>();

        public FraudProtectionIOModel(object request, object response, string name = "")
        {
            Add(request, response, name);
        }

        public void Add(object request, object response, string name = "")
        {
            RequestResponsePairs.Add((
                JsonConvert.SerializeObject(request, Formatting.Indented),
                JsonConvert.SerializeObject(response, Formatting.Indented),
                name
                ));
        }
    }
}