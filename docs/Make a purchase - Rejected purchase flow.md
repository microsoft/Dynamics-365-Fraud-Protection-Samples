# Microsoft Dynamics 365 Fraud Protection - API examples
## Make a purchase - Rejected purchase flow

After sending Dynamics 365 Fraud Protection a purchase event, use your merchant rule decision returned by Dynamics 365 Fraud Protection to either continue with or stop the purchase event workflow. As shown in the following examples, you ultimately inform Dynamics 365 Fraud Protection that a purchase was rejected.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Purchase - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/v1.0/V1.0MerchantservicesEventsPurchasePost)
- [Bank event - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/v1.0/V1.0MerchantservicesEventsBankEventPost)
- [Purchase status - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/v1.0/V1.0MerchantservicesEventsPurchaseStatusPost)
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
- [Make a Purchase - existing eser](./Make&#32;a&#32;purchase&#32;-&#32;Existing&#32;user.md)
- [Make a Purchase - guest user](./Make&#32;a&#32;purchase&#32;-&#32;Guest&#32;user.md)

## Declined bank auth event
The following example request sends a bank auth event to Dynamics 365 Fraud Protection informing it that the bank authorization was declined by the bank. If you send a declined bank auth, you would likely skip the bank charge event, and send a rejected purchase status event.
```http
POST <Merchant API Endpoint>/v1.0/MerchantServices/events/BankEvent HTTP/1.1
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID 1>
x-ms-dfpenvid: <enviroment ID>

{
  "bankEventId": "<merchant mastered bank event ID>",
  "type": "Auth",
  "bankEventTimestamp": "<timestamp from bank in ISO 8601 format>",
  "status": "Declined",
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

## Declined bank charge event
The following example request sends a bank charge event to Dynamics 365 Fraud Protection informing it that the bank charge was declined. If you send a declined bank charge, you would likely send a rejected purchase status event.
```http
POST <Merchant API Endpoint>/v1.0/MerchantServices/events/BankEvent HTTP/1.1
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID 1>
x-ms-dfpenvid: <enviroment ID>

{
  "bankEventId": "<merchant mastered bank event ID>",
  "type": "Charge",
  "bankEventTimestamp": "<timestamp from bank in ISO 8601 format>",
  "status": "Declined",
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

## Rejected purchase status event
The following example request sends a purchase status event to Dynamics 365 Fraud Protection informing it that the purchase was rejected from your perspective. You may send this immediately after getting a reject recommendation from your Dynamics 365 Fraud Protection merchant rule decision in a purchase event response. Additionally, you may send it in response to a declined bank auth or charge, or any other reason that you decide should reject a purchase, and need to inform Dynamics 365 Fraud Protection.
```http
POST <Merchant API Endpoint>/v1.0/MerchantServices/events/PurchaseStatus HTTP/1.1
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID 1>
x-ms-dfpenvid: <enviroment ID>

{
  "purchaseId": "<related purchase ID>",
  "statusType": "Rejected",
  "statusDate": "<purchase status change date in ISO 8601 format>",
  "reason": "RuleEngine",
  "_metadata": {
    "trackingId": "<tracking ID 3>",
    "merchantTimeStamp": "<event date in ISO 8601 format>"
  }
}
```
