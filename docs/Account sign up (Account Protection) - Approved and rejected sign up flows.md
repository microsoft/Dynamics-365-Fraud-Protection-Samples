# Microsoft Dynamics 365 Fraud Protection - API examples
## Account sign up (Account Protection APIs) - Approved and rejected sign up flows

After sending Dynamics 365 Fraud Protection a sign up event, use your merchant rule decision returned by Dynamics 365 Fraud Protection to either continue with or stop your sign up workflow.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Sample site - Register a user](../src/Web/Controllers/AccountController.cs) (see Register POST method)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostSignUpAP method)

**NOTES**
- As the merchant, you decide when to call the SignUp API. It may be before or after you have created an account in your database. The sample site calls the SignUp API before it creates a user in its database and only creates the user in the database if it decides to approve the sign up. This implementation decision is guided by the sample site's behavior.
- In the sample site, an existing user's username is set to their email address which is also tied to how they log in to the sample site. Furthermore, the sample site ensures that no two customers have the same email address. This ensures usernames will be unique for the sample site, as well as unique when sent to Dynamics 365 Fraud Protection. Decide what format to use for existing user IDs. It does not have to be an email address, but you should be careful not to duplicate user IDs.

## Required data
- Sign up ID
- Customer local date
- Merchant local date
- Session ID
- Username

## Optional data
- User data (user ID, email, address, payment instrument, etc.)
- SSO Authentication Provider

## Sign up event
This example request sends a sign up event to Dynamics 365 Fraud Protection asking for a risk assessment of the sign up.
```http
POST <Merchant API Endpoint>/v0.5/merchantservices/AccountProtection/events/<Merchant Instance ID>/AccountCreation/<Sign Up ID>
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID 1>

{
  "User": {
    "UserId": null,
    "UserType": "Consumer",
    "Username": "email@test.com",
    "PasswordHash": "<Hash of the user's password.  The sample app uses the UserManager.PasswordHasher.HashPassword method to generate this value.>",
    "FirstName": "first",
    "LastName": "last",
    "Country": "US",
    "ZipCode": "98052",
    "TimeZone": "-08:00:00",
    "Language": "EN-US",
    "MembershipId": null
  },
  "SSOAuthenticationProvider": {
    "authenticationProvider": "MSA | Facebook | PSN | MerchantAuth | Google",
    "displayName": "customer display name"
  },
  "Email": {
    "EmailType": "Primary",
    "EmailValue": "email@test.com",
    "IsEmailValidated": false,
    "IsEmailUsername": true
  },
  "Phone": {
    "PhoneType": "Primary",
    "PhoneNumber": "1231231234",
    "IsPhoneNumberValidated": false,
    "IsPhoneUsername": false
  },
  "Address": {
    "AddressType": "Primary",
    "FirstName": "first",
    "LastName": "last",
    "PhoneNumber": "1231231234",
    "Street1": "address1",
    "Street2": "address2",
    "Street3": null,
    "City": "Redmond",
    "State": "WA",
    "District": null,
    "ZipCode": "98052",
    "Country": "US"
  },
  "Device": {
    "SessionId": "<session ID from device fingerprinting>",
    "IpAddress": "0.0.0.1",
    "Provider": "DFPFingerPrinting"
  },
  "Metadata": {
    "SignUpId": "<merchant mastered sign up event ID>",
    "CustomerLocalDate": "<customer local date in ISO 8601 format>",
    "TrackingId": null,
    "MerchantTimeStamp": "<merchant local date in ISO 8601 format>"
  },
  "Name": "AP.AccountCreation",
  "Version": "0.5"
}
```