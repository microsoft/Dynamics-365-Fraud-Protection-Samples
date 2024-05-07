// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Entities.FraudProtectionApiModels.Response;
using System.Collections.Generic;
using System.Text.Json;

namespace Contoso.FraudProtection.Web.ViewModels
{
    public class FraudProtectionIOModel
    {
        public const string TempDataKey = "FraudProtectionIOData";

        public string CorrelationId { get; set; }

        public List<RequestResponsePair> RequestResponsePairs { get; set; } = new List<RequestResponsePair>();

        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        /// <summary>
        /// For serialization only
        /// </summary>
        public FraudProtectionIOModel() { }

        public FraudProtectionIOModel(string correlationId, object request, SampleResponseRaw response, string name = "", bool skipSerialization = false)
        {
            CorrelationId = correlationId;
            Add(request, response, name, skipSerialization);
        }

        public void Add(object request, SampleResponseRaw response, string name = "", bool skipSerialization = false)
        {
            RequestResponsePairs.Add(new RequestResponsePair
            {
                Request = skipSerialization ? request as string : JsonSerializer.Serialize(request, JsonSerializerOptions),
                Response = JsonSerializer.Serialize(response.RawData, JsonSerializerOptions),
                Name = name,
                ResponseTime = response.ResponseTime
            });
        }
    }

    public class RequestResponsePair
    {
        public string Request { get; set; }
        public string Response { get; set; }
        public string Name { get; set; }
        public long ResponseTime {  get; set; }
    }
}