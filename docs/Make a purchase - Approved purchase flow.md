# Microsoft Dynamics 365 Fraud Protection - API examples
## Make a purchase – Approved purchase flow

After sending Dynamics 365 Fraud Protection a purchase event, it is up to you to use the Dynamics 365 Fraud Protection decision to either continue with or stop the purchase workflow. Inform Dynamics 365 Fraud Protection of the intermediate bank events and final purchase status for a purchase to improve Dynamics 365 Fraud Protection models.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Purchase - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/Events/V0.5MerchantservicesEventsPurchasePost)
- [Bank Event - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/Events/V0.5MerchantservicesEventsBankEventPost)
- [Purchase Status - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/Events/V0.5MerchantservicesEventsPurchaseStatusPost)
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
- Bank auth ID
- Bank charge ID
- Merchant local date

## Optional data
- Purchase data (user ID, email, etc.)
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
POST https://<Merchant API Endpoint>/v1.0/MerchantServices/events/BankEvent HTTP/1.1
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID 1>

{
  "bankEventId": "<merchant mastered bank event ID>",
  "type": "Auth",
  "bankEventTimestamp": "<timestamp from bank in ISO 8601 format>",
  "status": "Approved",
  "bankResponseCode": "<bank response code>",
  "paymentProcessor": "<payment processor name>",
  "mrn": "<merchant reference number>",
  "mid": "<merchant ID sent to bank>",
  "purchaseId": "<related purchase ID>",
  "_metadata": {
    "trackingId": "<tracking ID 1>",
    "merchantTimeStamp": "<event date in ISO 8601 format>"
  }
}
```

## Bank charge event
This example request sends a bank charge event to Dynamics 365 Fraud Protection informing it that the bank charge was successful.
```http
POST https://<Merchant API Endpoint>/v1.0/MerchantServices/events/BankEvent HTTP/1.1
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID 1>

{
  "bankEventId": "<merchant mastered bank event ID>",
  "type": "Charge",
  "bankEventTimestamp": "<timestamp from bank in ISO 8601 format>",
  "status": "Approved",
  "bankResponseCode": "<bank response code>",
  "paymentProcessor": "<payment processor name>",
  "mrn": "<merchant reference number>",
  "mid": "<merchant ID sent to bank>",
  "purchaseId": "<related purchase ID>",
  "_metadata": {
    "trackingId": "<tracking ID 2>",
    "merchantTimeStamp": "<event date in ISO 8601 format>"
  }
}
```

## Purchase status event
This example request sends a purchase status event to Dynamics 365 Fraud Protection informing it that the purchase was successful from your perspective. The purchase status may differ from bank event statuses and is completely up to you to determine.
```http
POST https://<Merchant API Endpoint>/v1.0/MerchantServices/events/PurchaseStatus HTTP/1.1
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID 1>

{
  "purchaseId": "<related purchase ID>",
  "statusType": "Approved",
  "statusDate": "<purchase status change date in ISO 8601 format>",
  "reason": "RuleEngine",
  "_metadata": {
    "trackingId": "<tracking ID 3>",
    "merchantTimeStamp": "<event date in ISO 8601 format>"
  }
}
```
