// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Dynamics.FraudProtection.Models
{
    public enum PaymentInstrumentType
    {
        CreditCard,
        DirectDebit,
        PayPal,
        MobileBilling,
        OnlineBankTransfer,
        Invoice,
        MerchantGiftCard,
        MerchantWallet,
        CashOnDelivery,
        Paytm,
        CCAvenue,
        Other
    }

    public enum PaymentInstrumentState
    {
        Active,
        Blocked,
        Expired
    }

    public enum DeviceContextProvider
    {
        DFPFingerPrinting,
        Merchant
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
        AppStore,
        Web,
        App
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

    public enum AuthenticationProviderType
    {
        MSA,
        Facebook,
        PSN,
        MerchantAuth,
        Google
    }
}
