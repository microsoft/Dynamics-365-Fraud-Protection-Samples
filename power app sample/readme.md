![banner](https://i.ibb.co/s6KQBB7/banner.png)

## Overview

This is a fully packaged solution for the **Dynamics 365 Fraud Protection Portal Admin App** that serves multiple purposes:

- It demonstrates the extensibility capabilities of Fraud Protection Event Tracing and provides an open-source reference for those creating their own custom Power App.
- It provides IT Engineers and Fraud Managers with a toolset for actively monitoring latency measurements for their Fraud Protection instance as well as for maintaining an interactive audit log of Fraud Protection portal related events.

The app works directly with Fraud Protection **Audit Events** and **Latency Events** data.

## Prerequisites

- [Azure](https://azure.microsoft.com/en-us/features/azure-portal/) Subscription
- Two [Event Hubs](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-create)
- [Dynamics 365 Fraud Protection event tracing subscriptions](https://docs.microsoft.com/en-us/dynamics365/fraud-protection/event-tracing) connected to your two Event Hubs (one for **Audit Events** and one for **Latency Events**)
- [Power Apps](https://powerapps.microsoft.com/en-us/) subscription
- Privileges to create new entities inside CDS (Check you can do so at the [Power Apps Portal](https://make.preview.powerapps.com/) under Data > Entities)

For help with setting up the above prerequisites, see [Dynamics 365 Fraud Protection Event Tracing](https://docs.microsoft.com/en-us/dynamics365/fraud-protection/event-tracing).

## Import solution

1. Download and save the [D365FraudProtectionPortalAdminPowerAppSolution.zip](https://github.com/microsoft/Dynamics-365-Fraud-Protection-Samples/raw/master/power%20app%20sample/D365FraudProtectionPortalAdminPowerAppSolution.zip) file to your computer.
2. In the [Power Apps Portal](https://make.preview.powerapps.com/), navigate to the **Solutions** page in the left navigation.
3. Click **Import**, and then click **Choose File**
4. Select `D365FraudProtectionPortalAdminPowerAppSolution.zip` to load the file.
5. Click **Next**, and then click **Import**.
6. Upon completion, you will have imported the following:

- `Dynamics 365 Fraud Protection Portal Admin App` Power App
- `Audit Event` Common Data Service (CDS) Entity
- `Latency Event` Common Data Service (CDS) Entity
- `Setting` Common Data Service (CDS) Entity

The entities are required for the functionality of the Power App, because Fraud Protection event data is stored to these entities as new records via **Common Data Service (CDS)**.

## Setup Logic Apps to Save Event Data to Common Data Service

We must setup two **Logic Apps** to save **Audit Event** and **Latency Event** data from your Fraud Protection portal to CDS as they arrive in **Event Hubs**. CDS setup with your event data only needs to be done _once_ and can be used across **Power BI**, **Power Apps**, **Power Automate**, etc.

Start with the [Logic App Templates](https://github.com/microsoft/Dynamics-365-Fraud-Protection-Samples/tree/master/logic%20app%20templates) to quickly setup the base steps required for our two **Logic App** flows - one for **Audit Events**, and one for **Latency Events**.

**Audit Events Logic App**:

1. Start with the **Audit Events** Logic App template.
2. Add a **New Action** (after `ParseEvent` on the list) from **Common Data Service (CDS)** for **Create a new record**.
   ![create new record](https://i.ibb.co/dmsDcdB/Clean-Shot-2020-08-24-at-11-30-14.png).
3. Setup the fields _exactly_ as below:  
   ![audit fields](https://i.ibb.co/Y3Tv06W/Clean-Shot-2020-08-24-at-11-30-35.png)  
   Note: You may have to click **See more** when loading in dynamic content.

**Latency Events Logic App**:

1. Start with the **Latency Events** Logic App template.
2. Add a **New Action** (after `ParseEvent` on the list) from **Common Data Service (CDS)** for **Create a new record**.
3. Setup the fields _exactly_ as below:  
    ![latency fields](https://i.ibb.co/dD09WWT/Clean-Shot-2020-08-24-at-11-42-43.png)  
   Note: You may have to click **See more** when loading in dynamic content.  
   The value for `API Name` is an expression with the following value: `body('ParseEvent')?['dimensionValues']?[2]`

**Confirm data is being saved successfully**:

1. Confirm that your **Logic App** is running properly. In the left navigation, click **Overview** and then click **Runs history**:
   ![runs history](https://i.ibb.co/zF0Hrns/Clean-Shot-2020-08-19-at-11-03-43.png).
2. Confirm that event data is being stored in CDS properly. Check the **Data** section of your entity inside of the [Power Apps Portal](https://make.preview.powerapps.com/). Be sure to toggle the view to `Custom fields`. Check this for both the `Audit Event` and the `Latency Event` entities.
   ![cds data](https://i.ibb.co/ZxdhMRT/Clean-Shot-2020-08-19-at-11-06-19.png)

   Note: You may not see data immediately since your Logic Apps must take time to receive events, process them, and then save them to CDS.

## Edit or Run Power App

1. On the [Power Apps Portal](https://make.preview.powerapps.com/), navigate to **Apps** on the left navigation, and then select the `D365 Fraud Protection Portal Admin App`.
2. Click:

- **Edit** to modify the app in **Power Apps Studio**
- **Play** to launch the app.

Note: You may have to verify and authenticate certain connections (such as that to your CDS entities) to confirm that **Audit Events** and **Latency Events** are working correctly.
