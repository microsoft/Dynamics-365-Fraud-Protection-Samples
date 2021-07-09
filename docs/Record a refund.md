# Microsoft Dynamics 365 Fraud Protection - API examples
## Record a refund

Customers may request refunds from you. To improve Dynamics 365 Fraud Protection models, inform it about these refunds. When refunds start and complete, notify Dynamics 365 Fraud Protection.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Refund - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/v1.0/V1.0MerchantservicesEventsRefundPost)
- [Sample site - Send refund event](../src/Web/Areas/Admin/Controllers/ManageController.cs) (see POST to Edit method)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostRefund method)

## Required data
- Refund ID
- Purchase ID

## Optional data
- User ID
- Refund details (amount, currency, reason, etc.)

**NOTE:** To send a refund from the sample site, click **Refund** from the **My orders** page.

## Example refund event
This example request sends a completed refund event to Dynamics 365 Fraud Protection:
```http
POST <Merchant API Endpoint>/v1.0/MerchantServices/events/Refund HTTP/1.1
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID>
x-ms-dfpenvid: <enviroment ID>

{
  "refundId": "<merchant mastered refund ID>",
  "reason": "CustomerRequest",
  "status": "Approved",
  "bankEventTimestamp": "<timestamp from bank in ISO 8601 format>",
  "amount": 65.10,
  "currency": "USD",
  "userId": "<user ID>",
  "purchaseId": "<related purchase ID>",
  "_metadata": {
    "trackingId": "<tracking ID>",
    "merchantTimeStamp": "<event date in ISO 8601 format>"
  }
}
```
