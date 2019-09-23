# Microsoft Dynamics 365 Fraud Protection - API examples
## Account sign up - Approved and rejected sign up flows

After sending Dynamics 365 Fraud Protection a sign up event, it is up to you to use the Dynamics 365 Fraud Protection risk score to either continue with or stop your sign up workflow. Inform Dynamics 365 Fraud Protection of the final sign up status to improve Dynamics 365 Fraud Protection models.

**Important:** Dynamics 365 Fraud Protection does not yet support rules for sign up assessment like it does for purchase assessment. Implement rules in your application to handle the sign up response risk score. For instance, you may decide that any score higher than some threshold is too risky and you will reject those sign ups. Once Dynamics 365 Fraud Protection supports sign up rules, configure those rules directly in Dynamics 365 Fraud Protection so that your applications can simply honor the merchant rule decision in the sign up response. This sample site randomly rejects 1 out of 3 sign ups to demonstrate the concept.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [SignUp - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/Events/v1.0/V1.0MerchantservicesEventsSignUpPost)
- [SignUp Status - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/Events/v1.0/V1.0MerchantservicesEventsSignUpStatusPost)
- [Sample site - Register a user](../src/Web/Controllers/AccountController.cs) (see Register POST method)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostSignUp and PostSignUpStatus methods)

## Sign up flow
The sign up event flow consists of:
1. Sign up event
1. Sign up status event

**NOTES**
- You are not required to send these events, much less send them in any particular order, or within any specific time. The sample site does, in fact, follow this order. 
- As the merchant, you decide when to call the SignUp API. It may be before or after you have created an account in your database. The sample site calls the SignUp API before it creates a user in its database and only creates the user in the database if it decides to approve the sign up. Notice how this sample site does not send a user ID until the sign up status event, and even then, only when it decides to approve the user. This implementation decision is guided by the sample site's behavior.
- Set the 'assessmentType' field based on if you plan to use the Dynamics 365 Fraud Protection risk score:
  - Pass 'Evaluate' if you do NOT plan to use the risk score and are still evaluating Dynamics 365 Fraud Protection against your existing fraud solution.
  - Pass 'Protect' if you plan to use the risk score . This creates more accurate reports when we can distinguish between your 'Evaluate' and 'Protect' API calls.
- In the sample site, an existing user's ID is set to their email address which is also tied to how they log in to the sample site. Furthermore, the sample site ensures that no two customers have the same email address. This ensures user IDs will be unique for the sample site, as well as unique when sent to Dynamics 365 Fraud Protection. Decide what format to use for existing user IDs. It does not have to be an email address, but you should be careful not to duplicate user IDs.

## Required data
- Sign up ID
- Sign up status
- Sign up status date
- Merchant local date

## Optional data
- User data (user ID, email, etc.)
- Marketing context data (info about what lead to the sign up attempt)
- Storefront context data (info about where the user is signing up)
- Sign up status reason

## Sign up event
This example request sends a sign up event to Dynamics 365 Fraud Protection asking for a risk assessment of the sign up.
```http
POST https://<Merchant API Endpoint>/v1.0/MerchantServices/events/SignUp HTTP/1.1
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID 1>

{
  "signUpId": "<merchant mastered sign up event ID>",
  "assessmentType": "<'Evaluate' or 'Protect' based on if you are using the Fraud Protection recommendation or not>",
  "customerLocalDate": "<customer local date in ISO 8601 format>",
  "merchantLocalDate": "<merchant local date in ISO 8601 format>",
  "user": {
    "address": {
      "firstName": "Tami",
      "lastName": "Shorts",
      "phoneNumber": "+1-1234567890",
      "street1": "123 State St",
      "city": "Bothell",
      "state": "WA",
      "zipCode": "98033",
      "country": "US"
    },
    "creationDate": "<user create date in ISO 8601 format>",
    "updateDate": "<user update date in ISO 8601 format>",
    "firstName": "Tami",
    "lastName": "Shorts",
    "country": "US",
    "zipCode": "98033",
    "timeZone": "-08:00:00",
    "language": "EN-US",
    "phoneNumber": "+1-1234567890",
    "email": "tami.shorts@microsoft.com",
    "profileType": "Consumer",
    "isEmailValidated": false,
    "isPhoneNumberValidated": false
  },
  "marketingContext": {
    "type": "Direct",
    "incentiveType": "None",
    "incentiveOffer": "Integrate with Fraud Protection"
  },
  "storeFrontContext": {
    "type": "Web",
    "storeName": "Fraud Protection Sample Site",
    "market": "US"
  },
  "deviceContext": {
    "deviceContextId": "<Device Fingerprinting ID>",
    "ipAddress": "115.155.53.248",
    "provider": "DFPFingerPrinting",
    "deviceContextDC": "uswest"
  },
  "_metadata": {
    "trackingId": "<tracking ID 1>",
    "merchantTimeStamp": "<event date in ISO 8601 format>"
  }
}
```

## Approved sign up status event
This example request sends a sign up status event to Dynamics 365 Fraud Protection informing it that the sign up was **approved** and providing the newly created user's ID.
```http
POST https://<Merchant API Endpoint>/v1.0/MerchantServices/events/SignUpStatus HTTP/1.1
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID 2>

{
  "signUpId": "<related sign up ID>",
  "statusType": "Approved",
  "statusDate": "<status change date in ISO 8601 format>",
  "reason": "User is Approved",
  "user": {
    "userId": "tami.shorts@microsoft.com"
  },
  "_metadata": {
    "trackingId": "<tracking ID 2>",
    "merchantTimeStamp": "<event date in ISO 8601 format>"
  }
}
```

## Rejected sign up status event
This example request sends a sign up status event to Dynamics 365 Fraud Protection informing it that the sign up was **rejected**. Notice that no user ID is sent.
```http
POST https://<Merchant API Endpoint>/v1.0/MerchantServices/events/SignUpStatus HTTP/1.1
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID 2>

{
  "signUpId": "<related sign up ID>",
  "statusType": "Rejected",
  "statusDate": "<status change date in ISO 8601 format>",
  "reason": "User is Rejected",
  "_metadata": {
    "trackingId": "<tracking ID 2>",
    "merchantTimeStamp": "<event date in ISO 8601 format>"
  }
}
```
