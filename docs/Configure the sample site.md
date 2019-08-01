# Microsoft Dynamics 365 Fraud Protection - API examples
## Configure the sample site

Follow these steps to configure the sample site before running it.

1. You must have already have an active Dynamics 365 Fraud Protection account. If you do not, please stop and contact Dynamics 365 Fraud Protection or your system integration partner.
1. If you haven't already, clone this repository locally.
1. In your local repository, open [appsettings.json](../src/Web/appsettings.json).
1. Go to the [Dynamics 365 Fraud Protection integration portal](https://dfp.microsoft-int.com).
1. Sign in and you will be on the dashboard.
1. If you aren't using Microsoft Device Fingerprinting, you can skip this step. If you are using Microsoft Device Fingerprinting or will in the future, follow the [Set up Azure DNS](https://docs.microsoft.com/en-us/dynamics365/fraud-protection/device-fingerprinting#set-up-azure-dns) steps from our product documentation.
   1. Put your merchant domain in the appsettings.json as the value for the "DeviceFingerprintingDomain" setting. 
   1. Copy your "Instance ID" from the portal dashboard's bottom right info card. It should resemble a GUID. Put it as the value for the "DeviceFingerprintingCustomerId" setting.
1. On the portal, go to the [Configuration --> Real Time APIs](https://dfp.microsoft-int.com/configuration/realTimeApis) page. Use it to set up API access via an Azure Active Directory (AAD) application:
   1. Note that you must be in one of these Azure roles to use this page successfully:
      1. [Application Administrator](https://docs.microsoft.com/en-us/azure/active-directory/users-groups-roles/directory-assign-admin-roles#application-administrator)
      1. [Cloud Application Administrator](https://docs.microsoft.com/en-us/azure/active-directory/users-groups-roles/directory-assign-admin-roles#cloud-application-administrator)
      1. [Global Administrator](https://docs.microsoft.com/en-us/azure/active-directory/users-groups-roles/directory-assign-admin-roles#company-administrator)
   1. Type a name for your application, such as "Dynamics 365 Fraud Protection service account - INT".
   1. Select "Integration" for the environment.
   1. Keep "Certificate" selected for the authentication method.
   1. Upload the public portion of a valid certificate you have and click "Create application".
   1. Copy the "Application (client) ID" and set it as the value for the "ClientId" setting.
   1. Copy the "Thumbprint" and set it as the value for the "CertificateThumbprint" setting.
   1. Install the matching private key on your local machine (or wherever you will run the sample site from) and place it in the "Current User" certificate store.
1. Go to the [Azure Admin Center](https://aad.portal.azure.com/#blade/Microsoft_AAD_IAM/ActiveDirectoryMenuBlade/Overview). Copy your tenant domain (e.g. contoso.onmicrosoft.com) and set the "Authority" setting to "https://login.microsoftonline.com/[your tenant domain]".
1. You've finished configuring the sample site. You can now run the sample site, which will make calls to the Dynamics 365 Fraud Protection APIs.

## Note: Authenticating with a password rather than a certificate
This sample application authenticates via a client ID/certificate pair. Using a client ID/password pair is also possible by using the ```ClientCredential``` C# class rather than the ```ClientAssertionCertificate``` class as in the TokenProviderService sample below. In this scenario, you would also create another AAD application using the Real Time APIs page, but select "Secret" as the authentication method. You would then need to safely and securely get that secret into the application.

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
      "Purchase": "/v0.5/MerchantServices/events/Transaction",
      "PurchaseStatus": "/v0.5/MerchantServices/events/PurchaseStatus",
      "BankEvent": "/v0.5/MerchantServices/events/BankEvent",
      "Chargeback": "/v0.5/MerchantServices/events/Chargeback",
      "Refund": "/v0.5/MerchantServices/events/Refund",
      "UpdateAccount": "/v0.5/MerchantServices/events/UpdateAccount"
    },
    "TokenProviderConfig": {
      "ClientId": "00112233-4455-6677-8899-aabbccddeefg",
      "Authority": "https://login.microsoftonline.com/contoso.onmicrosoft.com",
      "CertificateThumbprint": "934367bf1c97033f877db0f15cb1b586957d313"
    }
  }
}
```
