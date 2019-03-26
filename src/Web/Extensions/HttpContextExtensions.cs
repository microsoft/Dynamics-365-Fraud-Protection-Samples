// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Http;
using System;

namespace Microsoft.AspNetCore.Mvc
{
    public static class HttpContextExtensions
    {
        public static string GetSessionId(this IHttpContextAccessor context)
        {
            var sessionId = context.HttpContext.Session.GetString("session_id");
            if (string.IsNullOrEmpty(sessionId))
            {
                //Create the session ID if it has not yet been set.
                sessionId = Guid.NewGuid().ToString();
                context.HttpContext.Session.SetString("session_id", sessionId);
            }

            return sessionId;
        }
    }
}
