# Microsoft Dynamics 365 Fraud Protection - API examples
## Record a chargeback

While Dynamics 365 Fraud Protection tries to minimize chargebacks, they may still happen from time to time. To improve Dynamics 365 Fraud Protection models, inform it about chargebacks.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Chargeback - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/KnowledgeGatewayEvent/KnowledgeGatewayEventActivitiesChargebackPost)
- [Sample site - Send chargeback event](../src/Web/Controllers/OrderController.cs) (see ChargebackOrder method)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostChargeback method)

## Required data
- Chargeback ID
- Purchase ID
- Merchant local date

## Optional data
- User ID
- Chargeback details (amount, currency, reason, etc.)

**NOTE**

In the sample site, a user clicks **Chargeback** from their **Orders** page. In reality, customers would contact their bank, for instance, to request a chargeback, and the bank would ultimately inform you. At this point, you would send the chargeback event to Dynamics 365 Fraud Protection. You may negotiate with the bank to dispute the chargeback. In this instance, you can send multiple chargeback events to Dynamics 365 Fraud Protection with various statuses:
- "WON": You win the chargeback dispute.
- "LOST": You lose the chargeback dispute.
- "INITIATED": A chargeback starts, but is not finalized.

## Example chargeback event
The following example request sends a chargeback event to Dynamics 365 Fraud Protection. In this instance, you lost the chargeback dispute.
```http
POST https://api.dfp.microsoft.com/KnowledgeGateway/activities/Chargeback HTTP/1.1
Host: api.dfp.microsoft.com
Content-Type: application/json; charset=utf-8
x-ms-correlation-id: <correlation ID 1>
x-ms-tracking-id: <tracking ID 1>
Authorization: bearer <token>
Content-Length: <content length>

{
  "MerchantLocalDate": "<event date in ISO 8601 format>",
  "Data": {
    "ChargebackId": "<merchant mastered chargeback ID>",
    "Reason": "<reason for chargeback>",
    "Status": "LOST",
    "BankEventTimestamp": "<timestamp from bank in ISO 8601 format>",
    "Amount": 70.5,
    "Currency": "USD",
    "User": { "UserId": "<user ID>" },
    "Purchase": { "PurchaseId": "<purchase ID>" }
  }
}
```
