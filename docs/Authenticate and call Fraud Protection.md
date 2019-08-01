# Microsoft Dynamics 365 Fraud Protection - API examples
## Authenticate and call Dynamics 365 Fraud Protection

You must send an authentication token with Dynamics 365 Fraud Protection API calls. See the following example for one way to do that. It then walks you through the process of creating a request and handling the response.

## Helpful links
- [API spec](https://apidocs.microsoft.com/services/dynamics365fraudprotection)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs)
- [Configure the sample site](./Configure&#32;the&#32;sample&#32;site.md))
- [Integrate real-time APIs](https://go.microsoft.com/fwlink/?linkid=2085128)

## Authenticate with Dynamics 365 Fraud Protection API
This C# example below assumes you have already configured the sample site.

```csharp
public class TokenProviderService : ITokenProvider
{
    private TokenProviderServiceSettings _settings;

    public TokenProviderService(IOptions<TokenProviderServiceSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<string> AcquireTokenAsync(string resource)
    {
        var assertionCert = CertificateUtility.GetByThumbprint(_settings.CertificateThumbprint);
        var clientAssertion = new ClientAssertionCertificate(_settings.ClientId, assertionCert);
        
        var context = new AuthenticationContext(_settings.Authority);
        var authenticationResult = await context.AcquireTokenAsync(resource, clientAssertion);

        return authenticationResult.AccessToken;
    }
}
```

Behind the scenes, the code above generates an HTTP request and receives a response like below:

### Request
```http
POST <authority>/oauth2/token HTTP/1.1
Accept: application/json
Content-Type: application/x-www-form-urlencoded
Content-Length: <content length>
Host: login.microsoftonline.com

resource=https://api.dfp.microsoft.com
&client_id=<Azure Active Directory client app ID>
&client_assertion_type=urn:ietf:params:oauth:client-assertion-type:jwt-bearer
&client_assertion=<client secret; in this case a private cert>
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
  "expires_on":"<date timestamp>",
  "not_before":"<date timestamp>",
  "resource":"https://api.dfp.microsoft.com",
  "access_token":"<your access token; e.g.: eyJ0eXA...NFLCQ>"
}
```

## Token refreshing
Ensure your application gets a new access token as needed. For instance, when your existing one is about to expire. Many frameworks, including .NET Core seen in the C# sample above, handle this for you automatically by caching your access token and only getting a new one as needed.  

## Send events to Dynamics 365 Fraud Protection
All events sent to Dynamics 365 Fraud Protection follow the same JSON model:
```
{
    merchantLocalDate = <event date in ISO 8601 format>,
    Data = <event data>
}
```

> The update account event is the only exception, also requiring a ```customerLocalDate``` top-level property.

However, the event data model varies from one event type to the next.

For example, see the following request and response when sending a refund event to Dynamics 365 Fraud Protection.

### Request

```http
POST https://api.dfp.microsoft.com/v0.5/MerchantServices/events/Refund HTTP/1.1
Host: api.dfp.microsoft.com
Content-Type: application/json; charset=utf-8
x-ms-correlation-id: <correlation ID>
x-ms-tracking-id: <tracking ID>
Authorization: bearer <token>
Content-Length: <content length>

{
  "MerchantLocalDate": "<event date in ISO 8601 format>",
  "Data": {
    "RefundId": "<refund ID>",
    "Purchase": { "PurchaseId": "<purchase ID>" },
    "User": { "UserId": "<user ID>" },
    "Status": "COMPLETED",
    "BankEventTimestamp": "<refund date from bank in ISO 8601 format>",
    "Amount": 23.79,
    "Currency": "USD"
  }
}
```
### Response
```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: <date>
Content-Length: 27

{
  "resultDetails": {}
}
```

### Error response example 1
This response is the result of sending an expired/invalid access token:
```http
HTTP/1.1 401 Unauthorized
Content-Type: application/json
ErrorReason: TokenExpired
Date: Thu, 08 Nov 2018 08:57:07 GMT
Content-Length: 85

{
    "statusCode": 401,
    "message": "Unauthorized. Access token is missing or invalid."
}
```
### Error response example 2
This response is the result of sending a refund event without the required "user" property:
```http
HTTP/1.1 400 Bad Request
Content-Type: application/json
Date: Thu, 08 Nov 2018 08:44:02 GMT
Content-Length: 148

{
  "Code": null,
  "ErrorType": "ClientError",
  "Message": "Entity of type User is expected at //User",
  "Retryable": false,
  "Parameters": null,
  "OtherErrors": null
}
```
### Error response example 3
This response represents an intermittent error. In this case, retrying the request may be successful:
```http
HTTP/1.1 500 Internal Server Error
Content-Type: application/json
Date: Thu, 08 Nov 2018 09:01:51 GMT
Content-Length: 171

{
  "Code": null,
  "ErrorType": "ServerError",
  "Message": "Internal Server Error - please try again",
  "Retryable": true,
  "Parameters": null,
  "OtherErrors": null
}
```
