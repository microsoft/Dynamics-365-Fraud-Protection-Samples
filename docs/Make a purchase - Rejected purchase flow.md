# Microsoft Dynamics 365 Fraud Protection - API examples
## Make a purchase - Rejected purchase flow

After sending Dynamics 365 Fraud Protection a purchase event, you can use the Dynamics 365 Fraud Protection decision to either continue with or stop the purchase event workflow. As shown in the following examples, you ultimately inform Dynamics 365 Fraud Protection that a purchase was rejected.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Purchase - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/Events/V0.5MerchantservicesEventsPurchasePost)
- [Bank event - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/Events/V0.5MerchantservicesEventsBankEventPost)
- [Purchase status - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/Events/V0.5MerchantservicesEventsPurchaseStatusPost)
- [Sample site - Make a purchase](../src/Web/Controllers/BasketController.cs) (see SetupPurchase and CheckoutDetails methods)
- [Sample site - Sending bank and purchase status events](../src/Web/Controllers/BasketController.cs) (see ApproveOrRejectPurchase, SetupBankEvent, and SetupPurchaseStatus methods)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostPurchase, PostBankEvent, and PostPurchaseStatus methods)

## Rejected purchase flows
A typical purchase event flow consists of:
1. Purchase event
1. Bank auth event
1. Bank charge event
1. Purchase status event

However, in a rejected purchase flow, the bank auth and charge events might never occur; or they may, depending on the cause(s) for the purchase to be rejected. 

## Required data
- Purchase ID
- Purchase status
- Purchase status date
- User ID
- Bank auth ID
- Bank charge ID
- Merchant local date

## Optional data
- Bank auth data (status, response code, etc.)
- Bank charge data (status, response code, etc.)
- Purchase status reason

## Purchase event
Examples:
- [Make a Purchase - existing eser](./Make&#32;a&#32;purchase&#32;-&#32;Existing&#32;user.md)
- [Make a Purchase - guest user](./Make&#32;a&#32;purchase&#32;-&#32;Guest&#32;user.md)

## Rejected bank auth event
The following example request sends a bank auth event to Dynamics 365 Fraud Protection informing it that the bank authorization was rejected. If you send a rejected bank auth, you would likely skip the bank charge event, and send a rejected purchase status event.
```http
POST https://api.dfp.microsoft.com/KnowledgeGateway/activities/BankEvent HTTP/1.1
Host: api.dfp.microsoft.com
Content-Type: application/json; charset=utf-8
x-ms-correlation-id: <correlation ID 1>
x-ms-tracking-id: <tracking ID 1>
Authorization: bearer <token>
Content-Length: <content length>

{
  "MerchantLocalDate": "<event date in ISO 8601 format>",
  "Data": {
    "BankEventId": "<merchant mastered bank event ID>",
    "Type": "AUTH",
    "Status": "REJECTED",
    "BankEventTimestamp": "<timestamp from bank in ISO 8601 format>",
    "BankResponseCode": "<bank response code>",
    "PaymentProcessor": "<payment processor name>",
    "MID": "<merchant ID sent to bank>",
    "Purchase": { "PurchaseId": "<purchase ID>" }
  }
}
```

## Rejected bank charge event
The following example request sends a bank charge event to Dynamics 365 Fraud Protection informing it that the bank charge was rejected. If you send a rejected bank charge, you would likely send a rejected purchase status event.
```http
POST https://api.dfp.microsoft.com/KnowledgeGateway/activities/BankEvent HTTP/1.1
Host: api.dfp.microsoft.com
Content-Type: application/json; charset=utf-8
x-ms-correlation-id: <correlation ID 1>
x-ms-tracking-id: <tracking ID 2>
Authorization: bearer <token>
Content-Length: <content length>

{
  "MerchantLocalDate": "<event date in ISO 8601 format>",
  "Data": {
    "BankEventId": "<merchant mastered bank event ID>",
    "Type": "CHARGE",
    "Status": "REJECTED",
    "BankEventTimestamp": "<timestamp from bank in ISO 8601 format>",
    "BankResponseCode": "<bank response code>",
    "MID": "<merchant ID sent to bank>",
    "Purchase": { "PurchaseId": "<purchase ID>" }
  }
}
```

## Canceled purchase status event
The following example request sends a purchase status event to Dynamics 365 Fraud Protection informing it that the purchase was canceled from your perspective. You may send this immediately after getting a reject decision from Dynamics 365 Fraud Protection in a purchase event response. Additionally, you may send it in response to a rejected bank auth or charge, or any other reason that you decide to reject a purchase, and need to inform Dynamics 365 Fraud Protection.
```http
POST https://api.dfp.microsoft.com/KnowledgeGateway/activities/PurchaseStatus HTTP/1.1
Host: api.dfp.microsoft.com
Content-Type: application/json; charset=utf-8
x-ms-correlation-id: <correlation ID 1>
x-ms-tracking-id: <tracking ID 3>
Authorization: bearer <token>
Content-Length: <content length>

{
  "MerchantLocalDate": "<event date in ISO 8601 format>",
  "Data": {
    "PurchaseId": "<purchase ID>",
    "Status": {
      "StatusType": "CANCELED",
      "StatusDate": "<purchase status change date in ISO 8601 format>",
      "Reason": "Some reason for CANCELED"
    }
  }
}
```
