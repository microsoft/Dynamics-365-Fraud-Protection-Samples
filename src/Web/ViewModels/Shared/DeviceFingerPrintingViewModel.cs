// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;

namespace Contoso.FraudProtection.Web.ViewModels.Shared
{
    public class DeviceFingerPrintingViewModel
    {
        public int ClientTimeZone { get; set; }

        public DateTime ClientDate { get; set; }

        public string ClientCountryCode { get; set; }

        public string FingerPrintingDC { get; set; }

        public string SessionId { get; set; }
    }
}
