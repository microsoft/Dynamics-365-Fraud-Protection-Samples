# Microsoft Dynamics 365 Fraud Protection - API examples
## Send additional fraud signals using labels

You can send additional fraud signals to Microsoft Dynamics 365 Fraud Protection about the following:
- Fraudulent activity, account abuse, or payment instrument abuse identified by your review team.
- Fraudulent transactions escalated by your customers.
- TC40 / SAFE signals received.
- Offline analysis like behavior analysis or discovered connections to existing fraud cases.
- Account takeover scenarios escalated by your customers.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Label - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/v1.0/V1.0MerchantservicesEventsLabelPost)
- [Sample site - Send label event](../src/Web/Controllers/OrderController.cs) (see FraudLabelOrder POST method)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostLabel method)

## Required data
- Label object type
- Label object ID
- Label source

## Optional data
- Label details (state, reason, timestamp, start and end dates, etc)
- Purchase details (amount and currency)

**NOTE:**
In the sample site, click **Fraud Label** from the **My orders** page to send a label event. In reality, you will receive label data from a variety of sources and so it is up to you to identify those places in your system and and send corresponding label events.

## Example label event - TC40 / SAFE report related to a purchase
The following example request sends a label event to Dynamics 365 Fraud Protection. In this example, you have received a TC40 / SAFE report from a card issuer indicating a transaction may be potentially fraudulent. Let Dynamics 365 Fraud Protection know about this to improve fraud detection.
```http
POST https://<Merchant API Endpoint>/v1.0/MerchantServices/events/Label HTTP/1.1
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID>

{
  "labelObjectType": "Purchase",
  "labelObjectId": "<related purchase ID>",
  "labelSource": "TC40_SAFE",
  "labelReasonCodes": "FraudRefund",
  "labelState": "Accepted",
  "processor": "<processor name>",
  "eventTimeStamp": "<timestamp as report by the label source / processor in ISO 8601 format>",
  "amount": 65.10,
  "currency": "USD",
  "_metadata": {
    "trackingId": "<tracking ID>",
    "merchantTimeStamp": "<event date in ISO 8601 format>"
  }
}
```

## Example label event - Suspicious account activity
The following example request sends a label event to Dynamics 365 Fraud Protection. In this example, your own fraud review team suspects fraudulent account behavior. Let Dynamics 365 Fraud Protection know about this to improve fraud detection.
```http
POST https://<Merchant API Endpoint>/v1.0/MerchantServices/events/Label HTTP/1.1
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID>

{
  "labelObjectType": "Account",
  "labelObjectId": "<related user ID>",
  "labelSource": "ManualReview",
  "labelReasonCodes": "Abuse",
  "labelState": "Abuse",
  "processor": "<processor name>",
  "eventTimeStamp": "<timestamp as report by the label source / processor in ISO 8601 format>",
  "_metadata": {
    "trackingId": "<tracking ID>",
    "merchantTimeStamp": "<event date in ISO 8601 format>"
  }
}
```
