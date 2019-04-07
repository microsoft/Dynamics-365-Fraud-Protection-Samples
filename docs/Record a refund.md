# Microsoft Dynamics 365 Fraud Protection - API examples
## Record a refund

Customers may request refunds from you. To improve Dynamics 365 Fraud Protection models, inform it about these refunds. When refunds start and complete, notify Dynamics 365 Fraud Protection.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Refund - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/KnowledgeGatewayEvent/KnowledgeGatewayEventActivitiesRefundPost)
- [Sample site - Send refund event](../src/Web/Areas/Admin/Controllers/ManageController.cs) (see POST to Edit method)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostRefund method)

## Required data
- Refund ID
- Purchase ID
- Merchant local date

## Optional data
- User ID
- Refund details (amount, currency, reason, etc.)

**NOTE**
In the sample site, to request a refund, a user selects **Refund** from their **Orders** page. Subsequently, an admin uses the sample site and can either complete or reject the refund.

## Example refund event
This example request sends a completed refund event to Dynamics 365 Fraud Protection:
```http
POST https://api.dfp.microsoft.com/KnowledgeGateway/activities/Refund HTTP/1.1
Host: api.dfp.microsoft.com
Content-Type: application/json; charset=utf-8
x-ms-correlation-id: <correlation ID 1>
x-ms-tracking-id: <tracking ID 1>
Authorization: bearer <token>
Content-Length: <content length>

{
  "MerchantLocalDate": "<event date in ISO 8601 format>",
  "Data": {
    "RefundId": "<merchant mastered refund ID>",
    "Status": "COMPLETED",
    "BankEventTimestamp": "<timestamp from bank in ISO 8601 format>",
    "Amount": 52.09,
    "Currency": "USD",
    "User": { "UserId": "<user ID>" },
    "Purchase": { "PurchaseId": "<purchase ID>" }
  }
}
```
