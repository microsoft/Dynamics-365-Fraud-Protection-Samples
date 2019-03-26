# Microsoft Dynamics 365 Fraud Protection - API examples
## Make a purchase – Guest user

You can request Dynamics 365 Fraud Protection for a risk decision when guest users make purchases. A guest user is simply a customer who has not registered with your business. The only difference in Dynamics 365 Fraud Protection between guest checkout and non-guest checkout is the user ID you send in the purchase event.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Purchase - Data model and endpoint](https://apidocs.microsoft.com/services/graphriskapi#/KnowledgeGatewayEvent/KnowledgeGatewayEventActivitiesPurchasePost)
- [Sample site - Make a purchase](../src/Web/Controllers/BasketController.cs) (see SetupPurchase and CheckoutDetails methods)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostPurchase method)

## Required data
- Purchase ID
- User ID
- Merchant local date

## Optional data
- Basic user data (name, email, phone, create/update date, etc.)
- Order details (tax, total, currency, etc.)
- Order line items
- Shipping address
- Payment instrument(s) details (credit card/PayPal/etc, billing addresses, etc.)
- Device info (IP, session ID, etc.)

[!NOTES]
- You should set the AssessmentType field based on if you plan to use the Dynamics 365 Fraud Protection risk recommendation:
  - Pass 'evaluate' if you do not plan to use the Dynamics 365 Fraud Protection risk recommendation, and are still evaluating Dynamics 365 Fraud Protection against your existing fraud solution.
  - Pass 'protect' if you plan to use the Dynamics 365 Fraud Protection risk recommendation. Consequently, Dynamics 365 Fraud Protection identifies that we must inform your bank about an incoming transaction via our Trusted MID program to potentially lift the bank acceptance rate. It also creates more accurate and detailed reports when we can distinguish between your 'evaluate' and 'protect' API calls.
- In the sample site, a guest user's ID is set to a random GUID to avoid matches with known or future user IDs. You can decide what format to use for guest user IDs. It doesn't have to be a random GUID, but be careful not to use an ID of a non-guest user. We do not recommend trying to detect if multiple guest customers are the same, real customer. Instead, include Microsoft device fingerprinting context in the purchase request.

## Example purchase request
```http
POST https://api.dfp.microsoft.com/KnowledgeGateway/activities/Purchase HTTP/1.1
Host: api.dfp.microsoft.com
Content-Type: application/json; charset=utf-8
x-ms-correlation-id: <correlation ID>
x-ms-tracking-id: <tracking ID>
Authorization: bearer <token>
Content-Length: <content length>

{
  "MerchantLocalDate": "<event date in ISO 8601 format>",
  "Data": {
    "PurchaseId": "<merchant mastered purchase ID>",
    "AssessmentType": "<'evaluate' or 'protect' based on if you are using the Fraud Protection recommendation or not>,
    "CustomerLocalDate": "<customer local date in ISO 8601 format>",
    "MerchantLocalDate": "<merchant local date in ISO 8601 format>",
    "TotalAmount": 76.49,
    "SalesTax": 5.99,
    "Currency": "USD",
    "ShippingMethod": "Standard",
    "User": {
      "UserId": "<Sample Site uses random, unique IDs for guest users, e.g. a new GUID>",
      "UserDetails": {
        "CreationDate": "<user create date in ISO 8601 format>",
        "UpdateDate": "<user update date in ISO 8601 format>",
        "FirstName": "Tami",
        "LastName": "Shorts",
        "Country": "US",
        "ZipCode": "98033",
        "TimeZone": "Pacific Standard Time",
        "Language": "EN",
        "PhoneNumber": "(455)-789-9875",
        "Email": "tami.shorts@microsoft.com",
        "ProfileType": "Consumer"
      }
    },
    "DeviceContext": {
      "DeviceContextId": "<Device Fingerprinting ID>",
      "IPAddress": "115.155.53.248",
      "DeviceContextDetails": {
        "DeviceContextDC": "uswest",
        "Provider": "DFPFINGERPRINTING",
        "ExternalDeviceId": ""
      }
    },
    "ShippingAddress": {
      "FirstName": "Tami",
      "LastName": "Shorts",
      "PhoneNumber": "(455)-789-9875",
      "ShippingAddressDetails": {
        "Street1": "123 State St",
        "City": "Bothell",
        "State": "WA",
        "ZipCode": "98033",
        "Country": "US"
      }
    },
    "PaymentInstrumentList": [
      {
        "PurchaseAmount": 76.49,
        "PaymentInstrumentDetails": {
          "Type": "CREDITCARD",
          "CreationDate": "<date credit card added to merchant's system in ISO 8601 format>",
          "State": "Active",
          "CardType": "Visa",
          "HolderName": "Tami Shorts",
          "BIN": "654398",
          "ExpirationDate": "02/23",
          "LastFourDigits": "8743",
          "BillingAddress": {
            "FirstName": "Tami",
            "LastName": "Shorts",
            "PhoneNumber": "(455)-789-9875",
            "BillingAddressDetails": {
              "Street1": "123 State St",
              "City": "Bothell",
              "State": "WA",
              "ZipCode": "98033",
              "Country": "US"
            }
          }
        }
      }
    ],
    "ProductList": [
      {
        "ProductId": "123",
        "PurchasePrice": 19.5,
        "Margin": 2.1,
        "Quantity": 3,
        "IsPreorder": false,
        "ShippingMethod": "Standard",
        "ProductDetails": {
          "ProductName": ".NET Bot Black Sweatshirt",
          "Type": "digital",
          "Sku": "1",
          "Category": "1",
          "Market": "US",
          "SalesPrice": 19.5,
          "Currency": "USD",
          "COGS": 0.11,
          "IsRecurring": false,
          "IsFree": false,
          "Language": "EN"
        }
      },
      {
        "ProductId": "245",
        "PurchasePrice": 12.0,
        "Margin": 2.1,
        "Quantity": 1,
        "IsPreorder": false,
        "ShippingMethod": "Standard",
        "ProductDetails": {
          "ProductName": "Prism White T-Shirt",
          "Type": "digital",
          "Sku": "2",
          "Category": "3",
          "Market": "US",
          "SalesPrice": 12.0,
          "Currency": "USD",
          "COGS": 0.11,
          "IsRecurring": false,
          "IsFree": false,
          "Language": "EN"
        }
      }
    ]
  }
}
```
