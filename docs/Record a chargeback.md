# Microsoft Dynamics 365 Fraud Protection - API Examples
## Record a chargeback

While Dynamics 365 Fraud Protection (Fraud Protection) tries to minimize chargebacks, they may still happen from time to time. You should inform Fraud Protection about chargebacks to improve Fraud Protection models.

## Helpful links
- [Calling Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Chargeback - Data model and endpoint](https://apidocs.microsoft.com/services/graphriskapi#/KnowledgeGatewayEvent/KnowledgeGatewayEventActivitiesChargebackPost)
- [Sample Site - Send chargeback event](../src/Web/Controllers/OrderController.cs) (see ChargebackOrder method)
- [Sample Site - Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostChargeback method)

## Required data
- Chargeback ID
- Purchase ID
- Merchant local date

## Optional data
- User ID
- Chargeback details (amount, currency, reason, etc.)

## Notes
In the Sample Site, a user clicks **Chargeback** from their **Orders** page. In reality, customers would contact their bank, for instance, to request a chargeback, and the bank would ultimately inform you. At this point, you would send the chargeback event to Fraud Protection. You may negotiate with the bank to dispute the chargeback. In this case, you can send multiple chargeback events to Fraud Protection with various statuses:
- "WON": You win the chargeback dispute.
- "LOST": You lose the chargeback dispute.
- "INITIATED": A chargeback starts, but is not finalized.

## Example chargeback event
This example request sends a chargeback event to Fraud Protection. In this case, you lost the chargeback dispute.
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
