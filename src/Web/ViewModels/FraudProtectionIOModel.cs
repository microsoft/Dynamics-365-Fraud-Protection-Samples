// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Text.Json;

namespace Contoso.FraudProtection.Web.ViewModels
{
    public class FraudProtectionIOModel
    {
        public const string TempDataKey = "FraudProtectionIOData";

        public string CorrelationId { get; set; }

        public string EnvironmentId { get; set; }

        public List<RequestResponsePair> RequestResponsePairs { get; set; } = new List<RequestResponsePair>();

        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        /// <summary>
        /// For serialization only
        /// </summary>
        public FraudProtectionIOModel() { }

        public FraudProtectionIOModel(string correlationId, object request, object response, string name = "", bool skipSerialization = false)
        {
            CorrelationId = correlationId;
            Add(request, response, name, skipSerialization);
        }

        public void Add(object request, object response, string name = "", bool skipSerialization = false)
        {
            RequestResponsePairs.Add(new RequestResponsePair
            {
                Request = skipSerialization ? request as string : JsonSerializer.Serialize(request, JsonSerializerOptions),
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