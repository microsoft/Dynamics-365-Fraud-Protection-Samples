# Microsoft Dynamics 365 Fraud Protection - API examples
## Make a purchase – Guest user

You can request Dynamics 365 Fraud Protection for a risk decision when guest users make purchases. A guest user is simply a customer who has not registered with your business. The only difference in Dynamics 365 Fraud Protection between guest checkout and non-guest checkout is the user ID you send in the purchase event.

## Helpful links
- [Calling Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- [Purchase - Data model and endpoint](https://apidocs.microsoft.com/services/dynamics365fraudprotection#/Events/V0.5MerchantservicesEventsPurchasePost)
- [Sample site - Make a purchase](../src/Web/Controllers/BasketController.cs) (see SetupPurchase and CheckoutDetails methods)
- [Sample site - Dynamics 365 Fraud Protection service](../src/Infrastructure/Services/FraudProtectionService.cs) (see PostPurchase method)

## Required data
- Purchase ID
- Merchant local date

## Optional data
- Basic user data (ID, name, email, phone, create/update date, etc.)
- Order details (tax, total, currency, etc.)
- Order line items
- Shipping address
- Payment instrument(s) details (credit card/PayPal/etc, billing addresses, etc.)
- Device info (IP, session ID, etc.)

**NOTES**
- You should set the AssessmentType field based on if you plan to use the Dynamics 365 Fraud Protection risk recommendation:
  - Pass 'Evaluate' if you do not plan to use the Dynamics 365 Fraud Protection risk recommendation, and are still evaluating Dynamics 365 Fraud Protection against your existing fraud solution.
  - Pass 'Protect' if you plan to use the Dynamics 365 Fraud Protection risk recommendation. Consequently, Dynamics 365 Fraud Protection identifies that we must inform your bank about an incoming transaction via our Trusted MID program to potentially lift the bank acceptance rate. It also creates more accurate and detailed reports when we can distinguish between your 'Evaluate' and 'Protect' API calls.
- In the sample site, a guest user's ID is set to a random GUID to avoid matches with known or future user IDs. You can decide what format to use for guest user IDs. It doesn't have to be a random GUID, but be careful not to use an ID of a non-guest user. We do not recommend trying to detect if multiple guest customers are the same, real customer. Instead, include Microsoft device fingerprinting context in the purchase request.

## Example purchase request
```http
POST https://<Merchant API Endpoint>/v1.0/MerchantServices/events/Purchase HTTP/1.1
x-ms-correlation-id: <correlation ID>
Content-Type: application/json; charset=utf-8
Authorization: bearer <token>
Content-Length: <content length>
Host: <Merchant API Endpoint>

{
	"purchaseId": "<merchant mastered purchase ID>",
	"assessmentType": "<'Evaluate' or 'Protect' based on if you are using the Fraud Protection recommendation or not>",
	"customerLocalDate": "2019-09-17T00:05:01.498+00:00",
	"merchantLocalDate": "2019-09-16T17:05:01.5619702-07:00",
	"totalAmount": 65.10,
	"salesTax": 5.10,
	"currency": "USD",
	"shippingMethod": "Standard",
	"user": {
		"userId": "tami.shorts2@microsoft.com",
		"creationDate": "2019-09-16T17:05:01.5612939-07:00",
		"updateDate": "2019-09-16T17:05:01.5613001-07:00",
		"firstName": "Tami",
		"lastName": "Shorts",
		"country": "US",
		"zipCode": "98033",
		"timeZone": "Pacific Standard Time",
		"language": "EN-US",
		"phoneNumber": "+1-1234567890",
		"email": "tami.shorts2@microsoft.com",
		"profileType": "Consumer",
		"isEmailValidated": false,
		"isPhoneNumberValidated": false
	},
	"deviceContext": {
		"deviceContextId": "744bfec4-5819-4a18-88da-985e62c4f53c",
		"ipAddress": "::1",
		"provider": "DFPFingerPrinting",
		"deviceContextDC": "uswest"
	},
	"shippingAddress": {
		"firstName": "Tami",
		"lastName": "Shorts",
		"phoneNumber": "+1-1234567890",
		"street1": "123 State St",
		"city": "Bothell",
		"state": "WA",
		"zipCode": "98033",
		"country": "US"
	},
	"paymentInstrumentList": [
		{
			"purchaseAmount": 65.10,
			"merchantPaymentInstrumentId": "tami.shorts2@microsoft.com-CreditCard",
			"type": "CreditCard",
			"creationDate": "2018-07-16T17:05:01.5614741-07:00",
			"state": "Active",
			"cardType": "Visa",
			"holderName": "Tami Shorts",
			"bin": "456978",
			"expirationDate": "03/21",
			"lastFourDigits": "6547",
			"billingAddress": {
				"firstName": "Tami",
				"lastName": "Shorts",
				"phoneNumber": "+1-1234567890",
				"street1": "123 State St",
				"city": "Bothell",
				"state": "WA",
				"zipCode": "98033",
				"country": "US"
			}
		}
	],
	"productList": [
		{
			"productId": "1",
			"productName": ".NET Foundation Sweatshirt",
			"type": "Digital",
			"sku": "1",
			"category": "ClothingShoes",
			"market": "US",
			"salesPrice": 12.0,
			"currency": "USD",
			"cogs": 0.11,
			"isRecurring": false,
			"isFree": false,
			"language": "EN-US",
			"purchasePrice": 12.0,
			"margin": 2.1,
			"quantity": 2,
			"isPreorder": false,
			"shippingMethod": "Standard"
		},
		{
			"productId": "2",
			"productName": "Cup<T> White Mug",
			"type": "Digital",
			"sku": "2",
			"category": "HomeGarden",
			"market": "US",
			"salesPrice": 12.0,
			"currency": "USD",
			"cogs": 0.11,
			"isRecurring": false,
			"isFree": false,
			"language": "EN-US",
			"purchasePrice": 12.0,
			"margin": 2.1,
			"quantity": 3,
			"isPreorder": false,
			"shippingMethod": "Standard"
		}
	],
	"_metadata": {
		"trackingId": "<tracking ID>",
		"merchantTimeStamp": "<event date in ISO 8601 format>"
	}
}


{
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
