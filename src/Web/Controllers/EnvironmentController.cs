// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contoso.FraudProtection.Web.Controllers
{
    public class EnvironmentController : Controller
    {
        [HttpPost]
        public IActionResult Update(string env)
        {
            HttpContext.Session.SetString("envId", env);
            return RedirectToAction("Index", "Catalog");
        }
    }
}
