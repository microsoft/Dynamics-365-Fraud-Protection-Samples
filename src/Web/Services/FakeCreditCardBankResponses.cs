// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Collections.Generic;

namespace Contoso.FraudProtection.Web.Services
{
    /// <summary>
    /// This entity mimics bank auth and charge responses so that appropriate Fraud Protection APIs can be called.
    /// In real life these APIs need to be invoked based on actual bank auth and charge responses.
    /// </summary>
    public class FakeCreditCardBankResponses
    {
        /// <summary>
        /// When set to true the demo assumes the bank auth is approved for a card. Rejected otherwise.
        /// </summary>
        public bool IsAuthApproved { get; set; }

        /// <summary>
        /// When set to true the demo assumes the bank charge is approved for a card. Rejected otherwise.
        /// </summary>
        public bool IsChargeApproved { get; set; }

        /// <summary>
        /// When set to true this demo approves the purchase transaction even when the Fraud Protection service recommends rejecting the purchase.
        /// </summary>
        public bool IgnoreFraudRiskRecommendation { get; set; }

        //Based on the names - the default behavior should be True True False or TTF (we auth approve, we approve charge, and we do not ignore Fraud Protection recommendation).
        //Also if auth is false we do not care what charge is. In cases where we do not care, we will put X.
        //Based on that logic we have 6 major cases:
        // TTF (default) -1111111111111111
        // TFF - 2222222222222222
        // FXF - 3333333333333333
        // TTT - 5555555555555555
        // TFT - 6666666666666666
        // FXT - 7777777777777777
        public static Dictionary<string, FakeCreditCardBankResponses> CreditCardResponses
        {
            get
            {
                return new Dictionary<string, FakeCreditCardBankResponses>()
                {
                    { "1111111111111111", new FakeCreditCardBankResponses {IsAuthApproved=true, IsChargeApproved=true, IgnoreFraudRiskRecommendation=false}},
                    { "2222222222222222", new FakeCreditCardBankResponses {IsAuthApproved=true, IsChargeApproved=false, IgnoreFraudRiskRecommendation=false}},
                    { "3333333333333333", new FakeCreditCardBankResponses {IsAuthApproved=false, IsChargeApproved=false, IgnoreFraudRiskRecommendation=false}},
                    { "5555555555555555", new FakeCreditCardBankResponses {IsAuthApproved=true, IsChargeApproved=true, IgnoreFraudRiskRecommendation=true}},
                    { "6666666666666666", new FakeCreditCardBankResponses {IsAuthApproved=true, IsChargeApproved=false, IgnoreFraudRiskRecommendation=true}},
                    { "7777777777777777", new FakeCreditCardBankResponses {IsAuthApproved=false, IsChargeApproved=false, IgnoreFraudRiskRecommendation=true}},
                };
            }
        }
    }
}
