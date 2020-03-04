# Microsoft Dynamics 365 Fraud Protection - API examples
## Account sign up (Account Protection APIs) - Approved and rejected sign up flows

After sending Dynamics 365 Fraud Protection a sign up event, use your merchant rule decision returned by Dynamics 365 Fraud Protection to either continue with or stop your sign up workflow.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [SignUp - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/v1.0/V1.0MerchantservicesEventsSignUpPost)
- [Sample site - Register a user](../src/Web/Controllers/AccountController.cs) (see Register POST method)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostSignUpAP method)

## Sign up flow
The sign up event flow consists of:
1. Sign up event

**NOTES**
- As the merchant, you decide when to call the SignUp API. It may be before or after you have created an account in your database. The sample site calls the SignUp API before it creates a user in its database and only creates the user in the database if it decides to approve the sign up. This implementation decision is guided by the sample site's behavior.
- In the sample site, an existing user's username is set to their email address which is also tied to how they log in to the sample site. Furthermore, the sample site ensures that no two customers have the same email address. This ensures usernames will be unique for the sample site, as well as unique when sent to Dynamics 365 Fraud Protection. Decide what format to use for existing user IDs. It does not have to be an email address, but you should be careful not to duplicate user IDs.

## Required data
- Sign up ID
- Customer local date
- Merchant local date
- Session ID
- Username
- Password hash

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
  "name": "AP.AccountCreation",
  "version": "0.5",
  "metadata": {
    "name": "AP.AccountCreation.Metadata",
    "signUpId": "34f47dc4-9781-4033-99fd-185649c4b001",
    "customerLocalDate": "2020-02-10T21:53:27.8760689-08:00",
    "trackingId": "e8a2f3cf-d3ef-4631-a276-893665c6cf54",
    "merchantTimeStamp": "2020-02-10T21:53:27.8822492-08:00"
  },
  "device": {
    "sessionId": "b2d36c49-e2ea-422d-acff-04798b85d520",
    "ipAddress": null,
    "provider": null,
    "externalDeviceId": null,
    "externalDeviceType": null
  },
  "user": {
    "userId": null,
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
  "email": {
    "emailType": "primary",
    "emailValue": "abc@my.com",
    "isEmailValidated": false,
    "emailValidatedDate": "0001-01-01T00:00:00",
    "isEmailUsername": false
  },
  "phone": {
    "phoneType": "alternative",
    "phoneNumber": "123-456-7890",
    "isPhoneNumberValidated": false,
    "phoneNumberValidatedDate": "2020-02-10T21:53:27.8833043-08:00",
    "isPhoneUsername": false
  },
  "address": {
    "addressType": "primary",
    "firstName": null,
    "lastName": null,
    "phoneNumber": null,
    "street1": null,
    "street2": null,
    "street3": null,
    "city": "Redmond",
    "state": null,
    "district": null,
    "zipCode": null,
    "country": "us"
  },
  "paymentInstrumentCard": {
    "merchantPaymentInstrumentId": "1234567890",
    "cardType": "mastercard",
    "holderName": "Don Flowers",
    "bin": "123456",
    "expirationDate": "12/2022",
    "lastFourDigits": "1234",
    "address": null,
    "name": "PaymentInstrument.Card",
    "state": "active"
  },
  "paymentInstrumentPaypal": {
    "email": "abc@me.com",
    "billingAgreementId": "123",
    "payerId": null,
    "payerStatus": null,
    "addressStatus": null,
    "name": "PaymentInstrument.Paypal",
    "state": "active"
  }
}
```