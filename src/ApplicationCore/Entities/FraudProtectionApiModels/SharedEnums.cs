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

    /// <summary>
    /// The possible assessment types.
    /// </summary>
    public enum AssessmentType
    {
        Evaluate,
        Protect
    }

    public enum StorefrontType
    {
        None,
        Web,
        Console,
        MobileApp,
        ComputerApp,
        MobileWeb
    }

    public enum MarketingType
    {
        None,
        Email,
        Referral,
        SearchEngine,
        Direct,
        SocialNetwork,
        Other
    }

    public enum MarketingIncentiveType
    {
        None,
        CashBack,
        Discount,
        FreeTrial,
        BonusPoints,
        Gift,
        Other
    }
}
