// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.ApplicationCore.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Contoso.FraudProtection.Web.Middleware
{
    public static class ExceptionHandlingMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "text/html";

                    var sb = new StringBuilder();
                    sb.AppendLine("<html lang='en'><body>");
                    sb.AppendLine("<h2><a href='/'>&larr; Back home</a></h2>");

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (exceptionHandlerPathFeature?.Error is FraudProtectionApiException e)
                    {
                        sb.AppendLine("<h2>Fraud Protection API error!</h2>");
                        using (e.Response)
                        {
                            sb.AppendLine("<h3>Request</h3>");
                            sb.AppendLine("<pre>");
                            sb.AppendLine(SanitizeAuthHeader(e.Response.RequestMessage.ToString()));
                            sb.AppendLine(await ReadContent(e.Response.RequestMessage.Content));
                            sb.AppendLine("</pre>");
                            sb.AppendLine("<h3>Response</h3>");
                            sb.AppendLine("<pre>");
                            sb.AppendLine(e.Response.ToString());
                            sb.AppendLine(await ReadContent(e.Response.Content));
                            sb.AppendLine("</pre>");
                        }
                    }
                    else
                    {
                        sb.AppendLine("<h2>Non Fraud Protection API error!</h2>");
                        sb.AppendLine("<a href='https://github.com/microsoft/Dynamics-365-Fraud-Protection-Samples/issues/new/choose' target='_blank'>Consider reporting this sample app bug</a><br />");
                        sb.AppendLine("<pre>");
                        sb.AppendLine(exceptionHandlerPathFeature?.Error.ToString());
                        sb.AppendLine("</pre>");
                    }

                    sb.AppendLine("</body></html>");

                    await context.Response.WriteAsync(sb.ToString());
                });
            });
        }

        private static async Task<string> ReadContent(HttpContent content)
        {
            var str = await content.ReadAsStringAsync();
            try
            {
                var obj = JsonSerializer.Deserialize<object>(str);
                return JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (JsonException)
            {
                return str;
            }
        }

        private static readonly Regex AuthRegex = new Regex("Authorization: bearer .*", RegexOptions.Compiled);
        private static string SanitizeAuthHeader(string content) => AuthRegex.Replace(content, "Authorization: bearer [removed]");
    }
}
