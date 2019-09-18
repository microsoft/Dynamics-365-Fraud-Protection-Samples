# Microsoft Dynamics 365 Fraud Protection - API examples
## Make a purchase - Handling the Dynamics 365 Fraud Protection purchase response

One of many core values of Dynamics 365 Fraud Protection is providing you a risk decision when you send it a purchase event. Use this decision to help guide your purchase workflow. For instance, you can display an error message to your customer if the risk decision indicates rejecting the purchase; oppositely, you can continue with the purchase workflow if the risk decision is to approve the purchase. You do not have to honor the risk decision, but we recommend doing so unless there are special considerations for specific purchases/customers. Even then, we recommend that you configure Dynamics 365 Fraud Protection rules in the Dynamics 365 Fraud Protection portal so that your risk decisions already include those custom rules.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Purchase - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/Events/V0.5MerchantservicesEventsPurchasePost)
- [Sample site - Handle the purchase response](../src/Web/Controllers/BasketController.cs) (see ApproveOrRejectPurchase method)

## Returned data
- Merchant rule decision: "Approve", "Reject", or "Pending".
- Dynamics 365 Fraud Protection risk score: Risk score for the purchase as determined by Dynamics 365 Fraud Protection.
- Dynamics 365 Fraud Protection reason codes: Risk attributes of the purchase as determined by Dynamics 365 Fraud Protection.
- Purchase ID: Same purchase ID that you sent to Dynamics 365 Fraud Protection to get this response.
- Merchant ID flag (MIDFlag): "Standard", "Program", or "Control". Use this flag to choose which Merchant ID (MID) you send to the bank during authorization. See the following summary of each MID:
  - **Standard**: Currently existing before integrating with Dynamics 365 Fraud Protection. 
  - **Program**: Used for high-confidence transactions expected to return a higher acceptance yield. 
  - **Control**: Provides a representative sample of performance before you start Dynamics 365 Fraud Protection, and will be used as a baseline for measuring overall gain.

Example HTTP response when Dynamics 365 Fraud Protection recommends that you **approve** a purchase:
```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
x-ms-correlation-id: 7f02a163-5fbb-4ed4-bb92-775473d03963
x-ms-appversion: 1.0:1.23:132104537452173612
Date: <date>
Content-Length: <content length>

{
  "resultDetails": {
    "MerchantRuleDecision": "Approve",
    "RiskScore": 15,
    "ReasonCodes": "500-LOCATION_CONSISTENCY:GENERAL,800-ACCOUNT_CONSISTENCY:GENERAL,1100-DEVICE_INFORMATION:GENERAL,700-ACCOUNT_INFORMATION:GENERAL",
    "MIDFlag": "Standard",
    "PolicyApplied": "Default",
    "PurchaseId": "<purchase ID>"
  }
}
```

Example HTTP response when Dynamics 365 Fraud Protection recommends that you **reject** a purchase: 
```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
x-ms-correlation-id: 7f02a163-5fbb-4ed4-bb92-775473d03964
x-ms-appversion: 1.0:1.23:132104537452173612
Date: <date>
Content-Length: <content length>

{
  "resultDetails": {
    "MIDFlag": "Standard",
    "PolicyApplied": "<merchant rule name, e.g. Reject very high scores>",
    "MerchantRuleDecision": "Reject",
    "RiskScore": 120,
    "ReasonCodes": "900-PAYMENT_INFORMATION:GENERAL,500-LOCATION_CONSISTENCY:GENERAL,1100-DEVICE_INFORMATION:GENERAL,700-ACCOUNT_INFORMATION:GENERAL"
    "PurchaseId": "<purchase ID>"
  }
}
```
