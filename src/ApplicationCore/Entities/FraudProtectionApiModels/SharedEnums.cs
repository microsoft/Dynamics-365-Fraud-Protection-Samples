// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Dynamics.FraudProtection.Models
{
    public enum PaymentInstrumentType
    {
        CREDITCARD,
        DEBITCARD,
        PAYPAL,
        MOBILEPAYMENT,
        GIFTCARD
    }

    public enum PaymentInstrumentState
    {
        Active,
        Blocked,
        Expired
    }

    public enum DeviceContextProvider
    {
        DFPFINGERPRINTING,
        MERCHANT
    }

    public enum UserProfileType
    {
        Consumer,
        Developer,
        Seller,
        Publisher,
        Tenant
    }
}
