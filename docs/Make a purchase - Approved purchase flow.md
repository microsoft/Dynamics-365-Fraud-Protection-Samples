# Microsoft Dynamics 365 Fraud Protection - API examples
## Make a purchase – Approved purchase flow

After sending Dynamics 365 Fraud Protection a purchase event, it is up to you to use the Dynamics 365 Fraud Protection decision to either continue with or stop the purchase workflow. Inform Dynamics 365 Fraud Protection of the intermediate bank events and final purchase status for a purchase to improve Dynamics 365 Fraud Protection models.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Purchase - Data model and endpoint](https://apidocs.microsoft.com/services/graphriskapi#/KnowledgeGatewayEvent/KnowledgeGatewayEventActivitiesPurchasePost)
- [Bank Event - Data model and endpoint](https://apidocs.microsoft.com/services/graphriskapi#/KnowledgeGatewayEvent/KnowledgeGatewayEventActivitiesBankEventPost)
- [Purchase Status - Data model and endpoint](https://apidocs.microsoft.com/services/graphriskapi#/KnowledgeGatewayEvent/KnowledgeGatewayEventActivitiesPurchasestatusPost)
- [Sample site - Make a purchase](../src/Web/Controllers/BasketController.cs) (see SetupPurchase and CheckoutDetails methods)
- [Sample site - Sending bank and purchase status events](../src/Web/Controllers/BasketController.cs) (see ApproveOrRejectPurchase, SetupBankEvent, and SetupPurchaseStatus methods)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostPurchase, PostBankEvent, and PostPurchaseStatus methods)

## Purchase Flow
A typical purchase event flow consists of:
1. Purchase event
1. Bank auth event
1. Bank charge event
1. Purchase status event

You typically send requests to your bank/payment gateway between some of these steps, so Dynamics 365 Fraud Protection identifies them as separate events. 

You are not required to send these events, much less send them in any particular order, or within any specific time. The sample site does, in fact, follow this order. 

**Important**

**Purchase updates not allowed**

After you send a purchase ID, you cannot send the same purchase ID again; purchase events cannot be updated like other Dynamics 365 Fraud Protection events. If you send the same purchase ID again, an error response is returned. You can update the bank auth, bank charge, and purchase status events as many times as needed, though.

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
- [Make a Purchase - Existing user](./Make&#32;a&#32;purchase&#32;-&#32;Existing&#32;user.md)
- [Make a Purchase - Guest user](./Make&#32;a&#32;purchase&#32;-&#32;Guest&#32;user.md)

## Bank auth event
This example request sends a bank auth event to Dynamics 365 Fraud Protection informing it that the bank authorization was successful.
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
    "Status": "APPROVED",
    "BankEventTimestamp": "<timestamp from bank in ISO 8601 format>",
    "BankResponseCode": "<bank response code>",
    "PaymentProcessor": "<payment processor name>",
    "MID": "<merchant ID sent to bank>",
    "Purchase": { "PurchaseId": "<purchase ID>" }
  }
}
```

## Bank charge event
This example request sends a bank charge event to Dynamics 365 Fraud Protection informing it that the bank charge was successful.
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
    "Status": "APPROVED",
    "BankEventTimestamp": "<timestamp from bank in ISO 8601 format>",
    "BankResponseCode": "<bank response code>",
    "MID": "<merchant ID sent to bank>",
    "Purchase": { "PurchaseId": "<purchase ID>" }
  }
}
```

## Purchase status event
This example request sends a purchase status event to Dynamics 365 Fraud Protection informing it that the purchase was successful from your perspective. The purchase status may differ from bank event statuses and is completely up to you to determine.
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
      "StatusType": "APPROVED",
      "StatusDate": "<purchase status change date in ISO 8601 format>",
      "Reason": "Some reason for APPROVED"
    }
  }
}
```
