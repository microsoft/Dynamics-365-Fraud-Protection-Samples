// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Microsoft.Dynamics.FraudProtection.Models.UpdateAccountEvent
{
    /// <summary>
    /// Updates or creates User account information, such as Add Payment Instrument, Add Address, or any other user attribute.
    /// </summary>
    public class User : UserDetails, IBaseFraudProtectionEvent
    {
        /// <summary>
        /// Payment instrument associated with this update account event
        /// </summary>
        public List<PaymentInstrument> PaymentInstrumentList { get; set; }

        /// <summary>
        /// Address associated with this update account event
        /// </summary>
        public List<UserAddress> AddressList { get; set; }

        /// <summary>
        /// Device associated with this update account event
        /// </summary>
        public DeviceContext DeviceContext { get; set; }

        // Inlining to avoid multi-inheritance but still take advantage of shared UserDetails
        [JsonProperty(PropertyName = "_metadata")]
        public EventMetadata Metadata { get; set; }
    }

    public enum UserAddressType
    {
        Shipping,
        Billing,
        Signup
    }

    /// <summary>
    /// Address associated with this update account event
    /// </summary>
    public class UserAddress : AddressDetails
    {
        /// <summary>
        /// See UserAddressType enum.
        /// </summary>
        public String Type { get; set; }
    }
}
