# Microsoft Dynamics 365 Fraud Protection - API Examples
## Create or update a user

You can inform Dynamics 365 Fraud Protection (Fraud Protection) when new customers register and create accounts. This call is optional, but providing this data helps build knowledge in Fraud Protection to better detect fraud when customers try to make purchases.

## Helpful links
- [Calling Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [UpdateUser - Data model and endpoint](https://apidocs.microsoft.com/services/graphriskapi#/KnowledgeGatewayEvent/KnowledgeGatewayEventActivitiesUpdateAccountPost)
- [Sample Site - Register a user](../src/Web/Controllers/ManageController.cs) (see POST to the Index method)
- [Sample Site - Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostUser method)

## Required data
- User ID
- Customer local date
- Merchant local date

## Optional data
- Basic user data (name, email, phone, create/update date, etc.)
- Regional info (country, timezone, language, etc.)
- Shipping address
- Billing address
- Payment method(s)
- Device info (IP, session ID, etc.)

## Notes
- In the Sample Site, the user's ID is set to their email. As the merchant, you can decide what you want to use for the user's ID. It does not have to be their email, but user IDs should be unique.
- In the Sample Site, the user's shipping and billing address are set to the same address, but they don't have to be.