# Microsoft Dynamics 365 Fraud Protection - API examples
## Account sign in (Account Protection APIs) - Approved and rejected sign in flows

After sending Dynamics 365 Fraud Protection a sign in event, use your merchant rule decision returned by Dynamics 365 Fraud Protection to either continue with or stop your sign in workflow.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Sample site - Sign in](../src/Web/Controllers/AccountController.cs) (see SignIn POST method)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostSignInAP method)

**NOTES**
- - Set the 'assessmentType' field based on if you plan to use the Dynamics 365 Fraud Protection risk score:
  - Pass 'Evaluate' if you do NOT plan to use the risk score and are still evaluating Dynamics 365 Fraud Protection against your existing fraud solution.
  - Pass 'Protect' if you plan to use the risk score . This creates more accurate reports when we can distinguish between your 'Evaluate' and 'Protect' API calls.
- In the sample site, an existing user's username is set to their email address which is also tied to how they log in to the sample site. Furthermore, the sample site ensures that no two customers have the same email address. This ensures usernames will be unique for the sample site, as well as unique when sent to Dynamics 365 Fraud Protection. Decide what format to use for existing user IDs. It does not have to be an email address, but you should be careful not to duplicate user IDs.

## Required data
- Login ID
- Customer local date
- Merchant local date
- Assessment type
- Session ID
- Username

## Optional data
- User data (first name, last name, zip code, etc.)
- SSO Authentication Provider
- Recent update dates (last phone number update date, last email update date, etc.)

## Sign in event
This example request sends a sign in event to Dynamics 365 Fraud Protection asking for a risk assessment of the sign in.
```http
POST <Merchant API Endpoint>/v1.0/action/account/login/<Sign In ID>
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID 1>
x-ms-dfpenvid: <enviroment ID>

{
  "Device": {
    "SessionId": "<session ID from device fingerprinting>",
    "IpAddress": "0.0.0.1",
    "Provider": "DFPFingerPrinting"
  },
  "User": {
    "UserId": null,
    "UserType": "Consumer",
    "Username": "email@test.com",
    "PasswordHash": "<Hash of the user's password.  The sample app uses the UserManager.PasswordHasher.HashPassword method to generate this value.>",
    "FirstName": "first",
    "LastName": "last",
    "CountryRegion": "US",
    "ZipCode": "98052",
    "TimeZone": "-08:00:00",
    "Language": "EN-US",
    "MembershipId": null
  },
  "SSOAuthenticationProvider": {
    "authenticationProvider": "MSA | Facebook | PSN | MerchantAuth | Google",
    "displayName": "customer display name"
  },
  "Metadata": {
    "LoginId": "<merchant mastered sign in event ID>",
    "CustomerLocalDate": "<customer local date in ISO 8601 format>",
    "AssessmentType": "<'Evaluate' or 'Protect' based on if you are using the Fraud Protection recommendation or not>",
    "TrackingId": null,
    "MerchantTimeStamp": "<merchant local date in ISO 8601 format>"
  },
  "RecentUpdate": {
    "lastPhoneNumberUpdateDate": "2020-02-10T21:53:27.8833043-08:00",
    "lastEmailUpdateDate": "2020-02-10T21:53:27.8833043-08:00",
    "lastAddressUpdateDate": "2020-02-10T21:53:27.8833043-08:00",
    "lastPaymentInstrumentUpdateDate": "2020-02-10T21:53:27.8833043-08:00"
  },
  "Name": "AP.AccountLogin",
  "Version": "0.5"
}
```