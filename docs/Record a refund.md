# Microsoft Dynamics 365 Fraud Protection - API Examples
## Record a refund

Customers may request refunds from you. You can inform Dynamics 365 Fraud Protection (Fraud Protection) about these refunds to improve Fraud Protection models. You can notify Fraud Protection when refunds start and when they complete.

## Helpful links
- [Calling Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Refund - Data model and endpoint](https://apidocs.microsoft.com/services/graphriskapi#/KnowledgeGatewayEvent/KnowledgeGatewayEventActivitiesRefundPost)
- [Sample Site - Send refund event](../src/Web/Areas/Admin/Controllers/ManageController.cs) (see POST to Edit method)
- [Sample Site - Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostRefund method)

## Required data
- Refund ID
- Purchase ID
- Merchant local date

## Optional data
- User ID
- Refund details (amount, currency, reason, etc.)

## Notes
In the Sample Site, a user can click **Refund** from their **Orders** page to request a refund. Subsequently, an admin must use the Sample Site and can either complete or reject the refund.

## Example refund event
This example request sends a completed refund event to Fraud Protection:
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
