// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Contoso.FraudProtection.Web.ViewModels.Shared;

namespace Contoso.FraudProtection.Web.ViewModels.Manage
{
    public class IndexViewModel
    {
        public UserViewModel User { get; set; }

        public AddressViewModel Address { get; set; }

        public DeviceFingerPrintingViewModel DeviceFingerPrinting { get; set; }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public string StatusMessage { get; set; }
    }
}
