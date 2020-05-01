// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Text.Json;

namespace Contoso.FraudProtection.Web.ViewModels
{
    public class FraudProtectionIOModel
    {
        public const string TempDataKey = "FraudProtectionIOData";

        public List<RequestResponsePair> RequestResponsePairs { get; set; } = new List<RequestResponsePair>();

        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        /// <summary>
        /// For serialization only
        /// </summary>
        public FraudProtectionIOModel() { }

        public FraudProtectionIOModel(object request, object response, string name = "")
        {
            Add(request, response, name);
        }

        public void Add(object request, object response, string name = "")
        {
            RequestResponsePairs.Add(new RequestResponsePair {
                Request = JsonSerializer.Serialize(request, JsonSerializerOptions),
                Response = JsonSerializer.Serialize(response, JsonSerializerOptions),
                Name = name
            });
        }
    }

    public class RequestResponsePair
    {
        public string Request { get; set; }
        public string Response { get; set; }
        public string Name { get; set; }
    }
}