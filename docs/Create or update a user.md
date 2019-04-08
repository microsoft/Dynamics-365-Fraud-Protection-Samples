# Microsoft Dynamics 365 Fraud Protection - API examples
## Create or update a user

You can inform Dynamics 365 Fraud Protection when new customers register and create accounts. Although this call is optional, we recommend providing this data to build knowledge in Dynamics 365 Fraud Protection to better detect fraud when customers try to make purchases.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [UpdateUser - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/Events/V0.5MerchantservicesEventsUpdateAccountPost)
- [Sample site - Register a user](../src/Web/Controllers/ManageController.cs) (see POST to the Index method)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostUser method)

## Required data
- User ID
- Customer local date
- Merchant local date

## Optional data
- Basic user data (name, email, phone, create/update date, etc.)
- Regional info (country, time zone, language, etc.)
- Shipping address
- Billing address
- Payment method(s)
- Device info (IP, session ID, etc.)

**Notes**
- In the sample site, the user's ID is set to their email. As the merchant, you can decide what you want to use for the user's ID. It does not have to be their email, but user IDs should be unique.
- In the sample site, the user's shipping and billing address are set to the same address, but this isn't mandatory.
