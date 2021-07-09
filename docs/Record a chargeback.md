# Microsoft Dynamics 365 Fraud Protection - API examples
## Record a chargeback

While Dynamics 365 Fraud Protection tries to minimize chargebacks, they may still happen from time to time. To improve Dynamics 365 Fraud Protection models, inform it about chargebacks.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Chargeback - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/v1.0/V1.0MerchantservicesEventsChargebackPost)
- [Sample site - Send chargeback event](../src/Web/Controllers/OrderController.cs) (see ChargebackOrder method)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostChargeback method)

## Required data
- Chargeback ID
- Purchase ID

## Optional data
- User ID
- Chargeback details (amount, currency, reason, etc.)

**NOTE:** 
In the sample site, a user clicks **Chargeback** from their **My orders** page. In reality, customers would contact their bank, for instance, to request a chargeback, and the bank would ultimately inform you. At this point, you would send the chargeback event to Dynamics 365 Fraud Protection. You may negotiate with the bank to dispute the chargeback. In this instance, you can send multiple chargeback events to Dynamics 365 Fraud Protection with various statuses:
- "Accepted": You, the merchant, accepted the chargeback.
- "Reversed": You, the merchant, won the chargeback dispute.
- "Disputed": You, the merchant, is disputing the chargeback. Until you send a chargeback update (e.g. reversed if you win the dispute) we will keep the previous status.
- "Inquiry": A pre-chargeback step. Customers can inquire about a transaction to get more details before filing a chargeback. Most of the time these turn into chargebacks.
- "ResubmittedRequest": The customer has requested another chargeback, even after a prior chargeback was reversed (you won the dispute).

## Example chargeback event
The following example request sends a chargeback event to Dynamics 365 Fraud Protection. In this instance, you accepted the chargeback.
```http
POST <Merchant API Endpoint>/v1.0/MerchantServices/events/Chargeback HTTP/1.1
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID>
x-ms-dfpenvid: <enviroment ID>

{
  "chargebackId": "<merchant mastered chargeback ID>",
  "reason": "Customer claims they didn't order this.",
  "status": "Accepted",
  "bankEventTimestamp": "<timestamp from bank in ISO 8601 format>",
  "amount": 13.02,
  "currency": "USD",
  "userId": "<user ID>",
  "purchaseId": "<related purchase ID>",
  "_metadata": {
    "trackingId": "<tracking ID>",
    "merchantTimeStamp": "<event date in ISO 8601 format>"
  }
}
```
