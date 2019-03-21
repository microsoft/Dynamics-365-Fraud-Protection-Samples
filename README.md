# Microsoft Dynamics 365 Fraud Protection - Sample Merchant App
This is the Microsoft Dynamics 365 Fraud Protection sample merchant application. It demonstrates how to call the [Fraud Protection APIs](https://apidocs.microsoft.com/services/graphriskapi) in the context of an online merchant who happens to sell clothing and other goods. This sample app may be useful if you are in the process of integrating with the APIs or if you want to see how new API endpoints/features should be integrated when there are new API versions.

It demonstrates two primary operations using the Fraud Protection APIs:
- Setting up the user and their associated information in the Fraud Protection system (e.g. basic info, payment methods, addresses, etc.).
- Requesting a Fraud Protection recommendation for a purchase and using that recommendation to decide whether to charge the customer.

Much of the code is based on the [eShopOnWeb](https://github.com/dotnet-architecture/eShopOnWeb) application.

## Local development setup
These prerequisites need to be installed prior to running the solution:

1. Visual Studio 2017 
2. .Net Core SDK version 2.1 or greater
3. (optional) SQL Management Studio, Azure Data Studio, or similar if you want to view your schema and data during testing/development.

## Running the application
```
To speed up local development, you can just start running/debugging the sample application locally and it will use an in-memory database, which lasts for that single run/debug session. Otherwise, follow the steps below to use your own database and then just run/debug the application.
```

### One time database setup:

1. Set the connection strings in `appsettings.json` and/or `appsettings.Development.json` to point to a database of your choosing. For instance, an Azure SQL Server database.
2. Open a command prompt in the src/Web folder and execute the following commands:

```
dotnet restore
dotnet ef database update -c catalogcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj
dotnet ef database update -c appidentitydbcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj
```

These commands populate two databases:
- One stores catalog and shopping cart information.
- One stores user credential and identity data.

The first time you run the application, it will seed both databases with data such that you should see products in the store, and you should be able to log in using the `demouser@microsoft.com` and `demoadminuser@microsoft.com` accounts detailed below.

### Additional database updates
The sample site uses .NET Core Entity Framework migrations. To add new migrations:
1. Open a command prompt in the src/Web folder and execute the following commands:
```
dotnet ef migrations add <migration name> -c catalogcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj -o Data/Migrations
dotnet ef migrations add <migration name> -c appidentitydbcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj -o Identity/Migrations
```

## Default login credentials

Admin:
```
User: demoadminuser@microsoft.com
Password: Pass@word1
```
Consumer:
``` 
User: demouser@microsoft.com
Password: Pass@word1
```

## Privacy and Telemetry
Once properly configured, this sample site uses Microsoft Device Fingerprinting to send device telemetry to Microsoft for the purposes of demonstrating Fraud Protection. To disable Microsoft Device Fingerprinting remove code related to it rather than configuring it. 

## Microsoft Open Source code of conduct
Please see the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct) for additional information.