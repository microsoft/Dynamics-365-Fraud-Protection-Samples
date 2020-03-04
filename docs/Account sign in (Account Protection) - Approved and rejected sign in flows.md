# Microsoft Dynamics 365 Fraud Protection - API examples
## Account sign in (Account Protection APIs) - Approved and rejected sign in flows

After sending Dynamics 365 Fraud Protection a sign in event, use your merchant rule decision returned by Dynamics 365 Fraud Protection to either continue with or stop your sign in workflow.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [SignIn - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/v1.0/V1.0MerchantservicesEventsSignUpPost)
- [Sample site - Sign in](../src/Web/Controllers/AccountController.cs) (see SignIn POST method)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostSignInAP method)

## Sign in flow
The sign in event flow consists of:
1. Sign in event

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
- Password hash

## Optional data
- User data (first name, last name, zip code, etc.)
- SSO Authentication Provider
- Recent update dates (last phone number update date, last email update date, etc.)

## Sign in event
This example request sends a sign in event to Dynamics 365 Fraud Protection asking for a risk assessment of the sign in.
```http
POST <Merchant API Endpoint>/v0.5/merchantservices/AccountProtection/events/<Merchant Instance ID>/AccountLogin/<Sign In ID>
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID 1>

{
  "name": "AP.AccountLogin",
  "version": "0.5",
  "metadata": {
    "loginId": "34f47dc4-9781-4033-99fd-185649c4b001",
    "customerLocalDate": "2020-02-10T21:53:27.8760689-08:00",
    "trackingId": "e8a2f3cf-d3ef-4631-a276-893665c6cf54",
    "merchantTimeStamp": "2020-02-10T21:53:27.8822492-08:00",
    "assessmentType": "evaluate | protect"
  },
  "device": {
    "sessionId": "b2d36c49-e2ea-422d-acff-04798b85d520",
    "ipAddress": null,
    "provider": null,
    "externalDeviceId": null,
    "externalDeviceType": null
  },
  "user": {
    "userId": "1234567890",
    "userType": "consumer",
    "username": "user_name",
    "passwordHash": "3q4w5e6r",
    "firstName": "Don",
    "lastName": "Joe",
    "country": "us",
    "zipCode": "98052",
    "timeZone": "PST",
    "language": "en-us",
    "membershipId": null,
    "isMembershipIdUsername": false
  },
  "ssoAuthenticationProvider": {
    "authenticationProvider": "MSA | Facebook | PSN | MerchantAuth | Google",
    "displayName": "customer display name"
  },
  "recentUpdate": {
    "lastPhoneNumberUpdateDate": "2020-02-10T21:53:27.8833043-08:00",
    "lastEmailUpdateDate": "2020-02-10T21:53:27.8833043-08:00",
    "lastAddressUpdateDate": "2020-02-10T21:53:27.8833043-08:00",
    "lastPaymentInstrumentUpdateDate": "2020-02-10T21:53:27.8833043-08:00"
  }
}
```