// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Contoso.FraudProtection.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> GetWithHeadersAsync(this HttpClient httpClient, string uri, Dictionary<string, string> headers)
        {
            return SendWithHeadersAsync(httpClient, uri, headers, HttpMethod.Get);
        }

        public static Task<HttpResponseMessage> PostWithHeadersAsync(this HttpClient httpClient, string uri, StringContent content, Dictionary<string, string> headers)
        {
            return SendWithHeadersAsync(httpClient, uri, headers, HttpMethod.Post, content);
        }

        public static Task<HttpResponseMessage> PutWithHeadersAsync(this HttpClient httpClient, string uri, StringContent content, Dictionary<string, string> headers)
        {
            return SendWithHeadersAsync(httpClient, uri, headers, HttpMethod.Put, content);
        }

        public static Task<HttpResponseMessage> DeleteWithHeadersAsync(this HttpClient httpClient, string uri, Dictionary<string, string> headers)
        {
            return SendWithHeadersAsync(httpClient, uri, headers, HttpMethod.Delete);
        }

        private static Task<HttpResponseMessage> SendWithHeadersAsync(this HttpClient httpClient, string uri, Dictionary<string, string> headers, HttpMethod method, StringContent content = null)
        {
            var request = new HttpRequestMessage()
            {
                Method = method,
                RequestUri = new Uri(uri),
            };

            if (content!=null)
            {
                request.Content = content;
            }

            foreach (string headerName in headers.Keys)
            {
                request.Headers.Add(headerName, headers[headerName]);
            }

            return httpClient.SendAsync(request);
        }
    }
}
