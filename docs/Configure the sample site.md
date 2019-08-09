# Microsoft Dynamics 365 Fraud Protection - API examples
## Configure the sample site

Follow these steps to configure the sample site before running it.

1. You must already have an active Dynamics 365 Fraud Protection account. If you do not, please stop and contact Dynamics 365 Fraud Protection or your system integration partner.
1. If you haven't already, clone this repository locally.
1. In your local repository, open [appsettings.json](../src/Web/appsettings.json).
1. Go to the [Dynamics 365 Fraud Protection integration portal](https://dfp.microsoft-int.com).
   1. If your Azure global administrator has not already visited the portal - and you are not an Azure global administrator - please ask them to do so first. They will need to agree to the terms of use in order to set up the integration environment.
1. Sign in and you will be on the dashboard.
1. Gather the following pieces of information from the dashboard:
   1. Copy the "Instance ID" GUID and set the "DeviceFingerprintingCustomerId" setting in your appsettings.json file to it.
   1. Copy the "Directory ID" GUID and set the "Authority" setting in your appsettings.json file to "https://login.microsoftonline.com/[Directory_ID]".
1. If you aren't using Microsoft Device Fingerprinting, you can skip this step. If you are using Microsoft Device Fingerprinting or will in the future, follow the [Set up Azure DNS](https://docs.microsoft.com/en-us/dynamics365/fraud-protection/device-fingerprinting#set-up-azure-dns) steps from our product documentation.
   1. Then, put your merchant domain name in your appsettings.json file as the value for the "DeviceFingerprintingDomain" setting. 
1. On the portal, go to the [Configuration --> Real Time APIs](https://dfp.microsoft-int.com/configuration/realTimeApis) page. Use it to set up API access via an Azure Active Directory (AAD) application:
   1. Note that you must be in one of these Azure roles to use this page successfully:
      1. [Application Administrator](https://docs.microsoft.com/en-us/azure/active-directory/users-groups-roles/directory-assign-admin-roles#application-administrator)
      1. [Cloud Application Administrator](https://docs.microsoft.com/en-us/azure/active-directory/users-groups-roles/directory-assign-admin-roles#cloud-application-administrator)
      1. [Global Administrator](https://docs.microsoft.com/en-us/azure/active-directory/users-groups-roles/directory-assign-admin-roles#company-administrator)
   1. Type a name for your application, such as "Dynamics 365 Fraud Protection service account - Integration".
   1. Select "Integration" for the environment.
   1. Keep "Certificate" selected for the authentication method.
   1. Upload the **public portion** of a valid certificate you have and click "Create application". The certificate can be self-signed or signed by a valid CA. Either will work.
   1. Copy the "Application (client) ID" and set it as the value for the "ClientId" setting in your appsettings.json file.
   1. Copy the "Thumbprint" and set it as the value for the "CertificateThumbprint" setting in your appsettings.json file.
   1. Install the matching private key on your local machine (or wherever you will run the sample site from) and place it in the "Current User" certificate store.
1. You've finished configuring the sample site. You can now run the sample site, which will make calls to the Dynamics 365 Fraud Protection APIs.

## Example: final appsettings.json
```json
{
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
  },
  "FraudProtectionSettings": {
    "DeviceFingerprintingDomain": "https://fpt.yourwebsite.com",
    "DeviceFingerprintingCustomerId": "00112233-4455-6677-8899-aabbccddeeff",
    "ApiBaseUrl": "https://api.dfp.microsoft-int.com",
    "Endpoints": {
      "Purchase": "/v0.5/MerchantServices/events/Purchase",
      "PurchaseStatus": "/v0.5/MerchantServices/events/PurchaseStatus",
      "BankEvent": "/v0.5/MerchantServices/events/BankEvent",
      "Chargeback": "/v0.5/MerchantServices/events/Chargeback",
      "Refund": "/v0.5/MerchantServices/events/Refund",
      "UpdateAccount": "/v0.5/MerchantServices/events/UpdateAccount"
    },
    "TokenProviderConfig": {
      "ClientId": "00112233-4455-6677-8899-aabbccddeefg",
      "Authority": "https://login.microsoftonline.com/11112222-3333-4444-5555-666677778888",
      "CertificateThumbprint": "111122223333444455556666777788889999000",
      "ClientSecret": ""
    }
  }
}
```

## Note: Authenticating with a password rather than a certificate
We recommend authenticating via certificate. If you want or need to authenticate via a secret (password) instead, select "Secret" on the Real Time APIs page instead of "Certificate". Then, copy the secret from the confirmation page and set it as the value for the "ClientSecret" setting in your appsettings.json file. For non sample apps, you would want to securely inject that secret into the application rather than having it hard-coded into your appsettings file.

## More info
Read [integrate real-time APIs](https://go.microsoft.com/fwlink/?linkid=2085128) for generalized information on configuring API access. The steps are nearly identical, but do not discuss the sample site's specific configuration file.