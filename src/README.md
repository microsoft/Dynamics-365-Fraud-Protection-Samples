# Microsoft Dynamics 365 Fraud Protection - Sample merchant application

The Microsoft Dynamics 365 Fraud Protection sample merchant application demonstrates how to call the [Dynamics 365 Fraud Protection APIs](https://apidocs.microsoft.com/services/dynamics365fraudprotection) in the context of an online merchant who sells clothing and other goods. This sample may be useful if you are in the process of integrating with the Dynamics 365 Fraud Protection APIs, or if you want to see how new API endpoints/features are integrated when there are new API versions.

## Local development setup

Before running the solution, you must install these prerequisites:

- Visual Studio 2022
- .Net Core SDK version 6.0 or greater
- (optional) SQL Management Studio, Azure Data Studio, or similar, if you want to view your schema and data during testing/development.

## Running the application

To speed up local development, you can start running/debugging the sample app locally. The app uses an in-memory database which lasts for that single run/debug session. Otherwise, follow these steps to use your own database, and then run/debug the app.

### One-time database setup

1. Set the connection strings in `appsettings.json` and/or `appsettings.Development.json` to point to a database of your choice (for example, an Azure SQL Server database).
2. Open a command prompt in the src/Web folder, and execute the following commands:

```
dotnet restore
dotnet ef database update -c catalogcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj
dotnet ef database update -c appidentitydbcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj
```

These commands populate two databases:

- One stores catalog and shopping cart information.
- One stores user credential and identity data.

The first time you run the application, it will seed both databases with data such that you should see products in the store, and be able to log in using the `demouser@microsoft.com` and `demoadminuser@microsoft.com` accounts. Details follow.

### Additional database updates

The sample site uses .NET Core Entity Framework migrations. To add new migrations, open a command prompt in the src/Web folder, and execute the following commands:

```
dotnet ef migrations add <migration name> -c catalogcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj -o Data/Migrations
dotnet ef migrations add <migration name> -c appidentitydbcontext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj -o Identity/Migrations
```

## Default login credentials

Use these accounts if you want to login without registering a new user account.

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

## Privacy and telemetry

Once properly configured, this sample site uses Microsoft device fingerprinting to send device telemetry to Microsoft for the purposes of demonstrating Dynamics 365 Fraud Protection. To disable device fingerprinting, remove code related to it rather than configuring it.
