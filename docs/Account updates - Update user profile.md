# Microsoft Dynamics 365 Fraud Protection - API examples
## Account updates - Update user profile

Inform Dynamics 365 Fraud Protection when customer accounts are updated. For example, a customer changes their shipping address or adds a new phone number to their account. Although this call is optional, we recommend providing this data to build knowledge in Dynamics 365 Fraud Protection to better detect fraud when customers try to make purchases.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [UpdateAccount - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/v1.0/V1.0MerchantservicesEventsUpdateAccountPost)
- [Sample site - Update account information](../src/Web/Controllers/ManageController.cs) (see POST to the Index method)
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
- In the sample site, the user's shipping and billing address are set to the same address, but this isn't mandatory.
- The UpdateAccount API creates a user if one does not already exist with the same user ID. The main differences with the SignUp API is that the SignUp API also does a risk assessment and does not collect as much user detail. If you do not want or need a risk assessment, but do want to create a user, you can call the UpdateAccount API.
