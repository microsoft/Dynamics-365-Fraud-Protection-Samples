# Microsoft Dynamics 365 Fraud Protection - API examples
## Configure the sample site

Follow these steps to configure the sample site before running it.

1. You must already have an active Dynamics 365 Fraud Protection account. If you do not, please stop and contact Dynamics 365 Fraud Protection or your system integration partner.
1. Sign in to the [Dynamics 365 Fraud Protection **integration** portal](https://dfp.microsoft-int.com). We will be using the integration environment for these instructions. You can use the production environment instead, but be careful not to send too much test data to it; a small amount is fine.
    1. If your Azure global administrator has not already visited the portal - and you are not an Azure global administrator - please ask them to do so first. They will need to agree to the terms of use in order to set up the environment.
1. Clone this repository locally and then...
    1. Open [Contoso.FraudProtection.sln](../Contoso.FraudProtection.sln), for instance in Visual Studio.
    1. Once loaded, open [appsettings.json](../src/Web/appsettings.json) in the Web project.
1. You can skip this step if you do not plan to use Microsoft Device Fingerprinting to help reduce fraud. To use Device Fingerprinting you will, in short, be referencing a JavaScript file in your web application. You have 2 options:
    1. Non-production setup - to test device fingerprinting works set the "DeviceFingerprintingDomain" setting to "https://fpt.dfp.microsoft.com" in your appsettings.json. This setup isn't recommend for production, but is perfectly suited to get familiar with device fingerprinting.
    1. Production setup - Follow the [Set up Azure DNS](https://docs.microsoft.com/en-us/dynamics365/fraud-protection/device-fingerprinting#set-up-azure-dns) steps from our product documentation. Then, set the "DeviceFingerprintingDomain" setting to your custom DNS name in your appsettings.json.
1. On the portal, go to [**Data --> API management --> Configuration**](https://dfp.microsoft-int.com/data/apimanagement/configuration). Use it to set up API access via an Azure Active Directory (AAD) application:
    1. Note that you must be in one of these Azure roles to use this page successfully:
        1. [Application Administrator](https://docs.microsoft.com/en-us/azure/active-directory/users-groups-roles/directory-assign-admin-roles#application-administrator)
        1. [Cloud Application Administrator](https://docs.microsoft.com/en-us/azure/active-directory/users-groups-roles/directory-assign-admin-roles#cloud-application-administrator)
        1. [Global Administrator](https://docs.microsoft.com/en-us/azure/active-directory/users-groups-roles/directory-assign-admin-roles#company-administrator)
    1. Copy the following information from this page:
        1. Copy "Instance ID" to the "InstanceId" and "DeviceFingerprintingCustomerId" setting in your appsettings.json file to it.
        1. Copy "Directory ID" to the "Authority" setting in your appsettings.json file to "https://login.microsoftonline.com/[Directory_ID]".
        1. Copy "API Resource URI" to the "Resource" setting in your appsettings.json file to it.
        1. Copy "API Endpoint" value to the "ApiBaseUrl" setting in your appsettings.json file to it.
    1. Type a name for your application, such as "Dynamics 365 Fraud Protection service account - Integration". This name is just for your use and can be anything you want.
    1. There are two ways to authenticate. It is up to you and your company policies/security which to use. *Secret is simpler if you just want to test your API connection:*
        1. **Secret**
            1.  Select "Secret" and click "Create application".
            1. Copy "Application (client) ID" to the "ClientId" setting in your appsettings.json file.
            1. Copy "Client secret" to the "ClientSecret" setting in your appsettings.json file.
```OR```
        a. **Certificate**
            a. Select "Certificate"
            b. Upload the **public key** of a valid certificate you have and click "Create application". The certificate can be self-signed or signed by a valid CA. Either will work.
            c. Copy "Application (client) ID" to the "ClientId" setting in your appsettings.json file.
            d. Copy "Thumbprint" to the "CertificateThumbprint" setting in your appsettings.json file.
            e. Install the matching private key on your local machine (or wherever you will run the sample site from) and place it in the "Current User" certificate store.
1. You've finished configuring the sample site. In Visual Studio **press F5 to start running the sample site** via the "eShopWeb" configuration.
    1. The sample site calls the Dynamics 365 Fraud Protection APIs from various pages such as Login, Register, and the Checkout flow. After making the calls, the site displays the requests and responses made on the bottom of the page.
    1. For Purchase Protection, beyond validating the API calls, you can use the [Graph Explorer](https://dfp.microsoft-int.com/data/explorer) or [Customer Support](https://dfp.microsoft-int.com/fraud/purchase/support) pages to search for your data and also view and validate Device Fingerprinting enrichments.
1. Ultimately, the steps you've taken, as well as the sample site code, should help you understand the steps needed to integrate your e-commerce sites with the production environment ([https://dfp.microsoft.com](https://dfp.microsoft.com)) APIs.

## Example: final appsettings.json
```json
{
  "FraudProtectionSettings": {
    "InstanceId": "00112233-4455-6677-8899-aabbccddeeff",
    "DeviceFingerprintingDomain": "https://fpt.yourwebsite.com",
    "DeviceFingerprintingCustomerId": "00112233-4455-6677-8899-aabbccddeeff",
    "ApiBaseUrl": "https://contoso-00112233-4455-6677-8899-aabbccddeeff.api.dfp.dynamics-int.com",
    "Endpoints": {
      "BankEvent": "/v1.0/MerchantServices/events/BankEvent",
      "Chargeback": "/v1.0/MerchantServices/events/Chargeback",
      "Label": "/v1.0/MerchantServices/events/Label",
      "Purchase": "/v1.0/MerchantServices/events/Purchase",
      "PurchaseStatus": "/v1.0/MerchantServices/events/PurchaseStatus",
      "Refund": "/v1.0/MerchantServices/events/Refund",
      "SignIn": "/v1.0/MerchantServices/events/SignIn",
      "Signup": "/v1.0/MerchantServices/events/Signup",
      "SignupStatus": "/v1.0/MerchantServices/events/SignUpStatus",
      "UpdateAccount": "/v1.0/MerchantServices/events/UpdateAccount",
      "SignInAP": "/v1.0/action/account/login/{0}",
      "SignupAP": "/v1.0/action/account/create/{0}",
      "CustomAssessment": "/v1.0/action/assessment/{0}"
    },
    "TokenProviderConfig": {
      "Resource": "https://api.dfp.dynamics-int.com",
      "ClientId": "00112233-4455-6677-8899-aabbccddeefg",
      "Authority": "https://login.microsoftonline.com/11112222-3333-4444-5555-666677778888",
      "CertificateThumbprint": "",
      "ClientSecret": "abcdefghijk"
    }
  },
    "ConnectionStrings": {
    "CatalogConnection": "<optional>",
    "IdentityConnection": "<optional>"
  },
  "CatalogBaseUrl": "",
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Warning",
      "Microsoft": "Warning",
      "System": "Warning"
    }
  }
}
```

## More info
Read [integrate real-time APIs](https://go.microsoft.com/fwlink/?linkid=2085128) for general information on configuring API access. The steps are nearly identical, but do not discuss the sample site's specific configuration file.
