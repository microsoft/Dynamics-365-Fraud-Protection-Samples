# Microsoft Dynamics 365 Fraud Protection - API examples
## Account updates - Update credit cards

Inform Dynamics 365 Fraud Protection when customers update their payment method(s). For example, your customer wants to add a new credit card to their account. This call is optional, but we recommend providing this data to build knowledge in Dynamics 365 Fraud Protection to better detect fraud when customers try to make purchases.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [UpdateAccount - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/v1.0/V1.0MerchantservicesEventsUpdateAccountPost)
- [Sample site - Manage a user](../src/Web/Controllers/ManageController.cs) (see POST to the ManagePaymentInstrument method)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostUser method)

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

**Note**
- The UpdateAccount API creates a user if one does not already exist with the same user ID. The main differences with the SignUp API is that the SignUp API also does a risk assessment and does not collect as much user detail. If you do not want or need a risk assessment, but do want to create a user, you can call the UpdateAccount API.
