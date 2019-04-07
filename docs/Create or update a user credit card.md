# Microsoft Dynamics 365 Fraud Protection - API examples
## Create or update a user credit card

You can inform Dynamics 365 Fraud Protection when customers update their payment method(s) (for example, your customer has a new credit card and wants to update it). This call is optional, but we recommend providing this data to build knowledge in Dynamics 365 Fraud Protection to better detect fraud when customers try to make purchases.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [UpdateUser - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/KnowledgeGatewayEvent/KnowledgeGatewayEventActivitiesUpdateAccountPost)
- [Sample Site - Manage a user](../src/Web/Controllers/ManageController.cs) (see POST to the ManagePaymentInstrument method)
- [Sample Site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostUser method)

## Required data
- User ID
- Credit card data
- Customer local date
- Merchant local date

## Optional data
- Basic user data (name, email, phone, create/update date, etc.)
- Regional info (country, time zone, language, etc.)
- Shipping address
- Billing address
- Device info (IP, session ID, etc.)
