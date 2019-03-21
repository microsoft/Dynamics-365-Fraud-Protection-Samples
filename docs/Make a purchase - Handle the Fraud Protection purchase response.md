# Microsoft Dynamics 365 Fraud Protection - API Examples
## Make a purchase - Handling the Fraud Protection purchase response

The core value of Dynamics 365 Fraud Protection (Fraud Protection) is providing you a risk decision when you send it a purchase event. Use this decision to help guide your purchase workflow. For instance, you can display an error message to your customer if the risk decision indicates rejecting the purchase; oppositely, continue with the purchase workflow if the risk decision is to approve the purchase. You do not have to honor the risk decision, but we recommend doing so unless there are special circumstances for specific purchases/customers. Even then, we recommend that you configure Fraud Protection rules in the Fraud Protection portal so that your risk decisions already include those custom rules.

## Helpful links
- [Calling Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Purchase - Data model and endpoint](https://apidocs.microsoft.com/services/graphriskapi#/KnowledgeGatewayEvent/KnowledgeGatewayEventActivitiesPurchasePost)
- [Sample Site - Handle the purchase response](../src/Web/Controllers/BasketController.cs) (see ApproveOrRejectPurchase method)

## Returned data
- Merchant Rule Decision: "Approve", "Reject", or "Pending".
- Fraud Protection Risk Score: The risk score for the purchase as determined by Fraud Protection.
- Fraud Protection Reason Codes: The risk attributes of the purchase as determined by Fraud Protection.
- Purchase ID: Same purchase ID that you sent to Fraud Protection to get this response.
- Merchant ID Flag (MIDFlag): "Default", "Program", or "Control". Use this flag to choose which Merchant ID (MID) you send to the bank during authorization. More information can be found in the Fraud Protection Onboarding document.  Here is a summary of each:
  - **Default**: Currently existing before integrating with Fraud Protection. 
  - **Program**: Used for high-confidence transactions expected to return a higher acceptance yield. 
  - **Control**: Provides a representative sample of performance before you start Fraud Protection, and will be used as a baseline for measuring overall gain.

Example HTTP response when Fraud Protection recommends that you **approve** a purchase:
```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: <date>
Content-Length: 98

{
  "resultDetails": {
    "MerchantRuleDecision": "Approve",
    "MIDFlag": "Control",
    "RiskScore": 2,
    "ReasonCodes": "",
    "PurchaseId": "<purchase id>"
  }
}
```

Example HTTP response when Fraud Protection recommends that you **reject** a purchase: 
```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: <date>
Content-Length: 98

{
  "resultDetails": {
    "MerchantRuleDecision": "Reject",
    "MIDFlag": "Control",
    "RiskScore": 93,
    "ReasonCodes": "RISKY PAYMENT METHOD,RISKY ACCOUNT LOCATION,SUSPICIOUS DEVICE IP,RISKY BILLING LOCATION,SUSPICIOUS NUMBER OF IPS FOR A DEVICE,RISKY PRODUCT",
    "PurchaseId": "<purchase id>"
  }
}
```
