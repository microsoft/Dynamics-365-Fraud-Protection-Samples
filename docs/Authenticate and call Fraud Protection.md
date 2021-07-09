# Microsoft Dynamics 365 Fraud Protection - API examples
## Authenticate and call Dynamics 365 Fraud Protection

You must send an authentication token with Dynamics 365 Fraud Protection API calls. See the following example for one way to do acquire a token. Sample API requests and responses are shown afterwards.

## Helpful links
- [API contracts](https://apidocs.microsoft.com/services/dynamics365fraudprotection)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs)
- [Configure the sample site](./Configure&#32;the&#32;sample&#32;site.md)
- [Integrate real-time APIs](https://go.microsoft.com/fwlink/?linkid=2085128)

## Authenticate with Dynamics 365 Fraud Protection API
Assuming you have [configured the sample site](./Configure&#32;the&#32;sample&#32;site.md), it will automatically acquire API tokens to use to call Dynamics 365 Fraud Protection APIs.

The [token provider service](../src/Infrastructure/Services/TokenProviderService.cs) acquires tokens using MSAL in C#. It generates an HTTP request and receives a response like below in the case of authenticating with a secret:

### Request
```http
POST <authority>/oauth2/v2.0/token HTTP/1.1

Accept: application/json
Content-Type: application/x-www-form-urlencoded
Content-Length: <content length>
Host: login.microsoftonline.com

client_id=<Azure Active Directory client app ID>
&client_info=1
&client_secret=<Azure Active Directory client app secret>
&scope=https://api.dfp.microsoft.com/.default
&grant_type=client_credentials
```

### Response
```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: <date>
Content-Length: <content length>

{
  "token_type":"Bearer",
  "expires_in":"3599",
  "ext_expires_in":"3599",
  "access_token":"<your access token; e.g.: eyJ0eXA...NFLCQ>"
}
```

## Token refreshing
Ensure your application gets a new access token as needed. For instance, when your existing one is about to expire. Many frameworks, including .NET Core seen in the C# sample above, handle this for you automatically by caching your access token and only getting a new one as needed.

## Send events to Dynamics 365 Fraud Protection
As an example, all Purchase Protection related events sent to Dynamics 365 Fraud Protection follow the same JSON model:
```
{
    "property 1": <value 1>,
    "property 2": <value 2>,
    ...
    "_metadata": {
      "trackingId": "<tracking ID>",
      "merchantTimeStamp": "<event date in ISO 8601 format>"
   }
}
```

The event data model varies from one event type to the next, but all have a _metadata property.

For example, see the following request and response when sending a refund event to Dynamics 365 Fraud Protection.

### Request

```http
POST <Merchant API Endpoint>/v1.0/MerchantServices/events/Refund HTTP/1.1
Host: <Merchant API Endpoint>
Authorization: bearer <token>
Content-Type: application/json; charset=utf-8
Content-Length: <content length>
x-ms-correlation-id: <correlation ID>
x-ms-dfpenvid: <enviroment ID>

{
  "refundId": "<merchant mastered refund ID>",
  "reason": "CustomerRequest",
  "status": "Approved",
  "bankEventTimestamp": "<timestamp from bank in ISO 8601 format>",
  "amount": 65.10,
  "currency": "USD",
  "userId": "<user ID>",
  "purchaseId": "<purchase ID>",
  "_metadata": {
    "trackingId": "<tracking ID>",
    "merchantTimeStamp": "<event date in ISO 8601 format>"
  }
}
```
### Response
```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: <date>
Content-Length: 49

{
  "resultDetails": {
    "Succeeded": true,
    "Error": null
  }
}
```

### Error response example 1
This response is the result of sending an expired/invalid access token:
```http
HTTP/1.1 401 Unauthorized
Content-Type: application/json
ErrorSource: validate-jwt
ErrorMessage: IDX10223: Lifetime validation failed. The token is expired. ValidTo: '09/20/2019 22:05:30', Current time: '09/20/2019 22:05:31'.
ErrorScope: global
ErrorSection: inbound
ErrorStatusCode: 401
ErrorReason: TokenExpired
Date: Fri, 20 Sep 2019 22:05:31 GMT
Content-Length: 85

{
  "statusCode": 401,
  "message": "Unauthorized. Access token is missing or invalid."
}
```

### Error response example 2
This response is the result of sending a sign up event without the required "signUpId" property:
```http
HTTP/1.1 400 Bad Request
Content-Type: application/json
Date: Fri, 20 Sep 2019 21:06:05 GMT
Content-Length: 163

{
  "ErrorType": "ClientError",
  "Message": "Input validation failed - Required properties signUpId are not present at level (root)",
  "Retryable": false,
  "OtherErrors": null
}
```

### Error response example 3
This response represents an intermittent error. In this case, retrying the request may be successful:
```http
HTTP/1.1 500 Internal Server Error
Content-Type: application/json
Date: Fri, 20 Sep 2019 21:06:10 GMT
Content-Length: 97

{
  "ErrorType": "ServerError",
  "Message": "Internal Server Error",
  "Retryable": true,
  "OtherErrors": null
}
```
