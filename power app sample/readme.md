![banner](https://i.ibb.co/QQCVq4m/banner.png)

## Overview

This is a fully packaged solution for the **D365 Fraud Protection Portal Admin App** that serves multiple purposes:

- Demonstrate the extensibility capabilities of Fraud Protection Event Tracing and provide an open-source reference for those creating their own custom Power App.
- Provide IT Engineers and Fraud Managers with a toolset for actively monitoring latency measurements for their Fraud Protection instance as well as maintaining an interactive audit log of Fraud Protection portal related events.

The app will work directly with Fraud Protection **Audit Events** and **Latency Events** data.

## Prerequisites

- Azure subscription
- Event Hub Namespace
- Two Event Hubs
- Dynamics 365 Fraud Protection event tracing subscriptions connected to your two Event Hubs (one for **Audit Events**, one for **Latency Events**)
- Power Apps subscription

For help setting up the above, check the documentation in **Reference Links** in the following section.

## Reference Links

[Dynamics 365 Fraud Protection Event Tracing Documentation](https://docs.microsoft.com/en-us/dynamics365/fraud-protection/event-tracing)

## Import Solution

1. Download and save the `D365FraudProtectionPortalAdminPowerAppSolution.zip` file in this repository to your computer.
2. In the [Power Apps Portal](https://make.preview.powerapps.com/), navigate to the **Solutions** page in the left sidebar.
3. Click **Import**, and in the window that is opened, click **Choose File** and load the `D365FraudProtectionPortalAdminPowerAppSolution.zip` file.
4. Click **Next**. Then, click **Import**.
5. Upon completion, you will have imported the following:

- `D365 Fraud Protection Portal Admin App` Power App
- `Audit Event` Common Data Service (CDS) Entity
- `Latency Event` Common Data Service (CDS) Entity
- `Setting` Common Data Service (CDS) Entity

The entities are required for the functionality of the Power App and we will store our Fraud Protection event data to these entities as new records via **Common Data Service (CDS)**.

## Setup Logic Apps to Save Event Data to Common Data Service

We must setup two **Logic Apps** to save **Audit Event** and **Latency Event** data from your Fraud Protection portal to CDS as they arrive in **Event Hubs**.

Start with the [Logic App Templates](https://github.com/microsoft/Dynamics-365-Fraud-Protection-Samples/tree/master/logic%20app%20templates) to quickly setup the base steps required for our two **Logic App** flows - one for **Audit Events**, one for **Latency Events**.

**Audit Events Logic App**:

1. Start with the **Audit Events** Logic App template.
2. Add a **New Action** after `ParseEvent` for **Create a new record** in **Common Data Service (CDS)**.
   ![create new record](https://i.ibb.co/dmsDcdB/Clean-Shot-2020-08-24-at-11-30-14.png).
3. Setup the fields in this action _exactly_ as the following:  
   ![audit fields](https://i.ibb.co/Y3Tv06W/Clean-Shot-2020-08-24-at-11-30-35.png)  
   Note: you may have to press **See More** when loading in dynamic content.

**Latency Events Logic App**:

1. Start with the **Latency Events** Logic App template.
2. Add a **New Action** after `ParseEvent` for **Create a new record** in **Common Data Service (CDS)**.
3. Setup the fields in this action _exactly_ as the following:  
    ![latency fields](https://i.ibb.co/dD09WWT/Clean-Shot-2020-08-24-at-11-42-43.png)  
   Note: you may have to press **See More** when loading in dynamic content.  
   The value for `API Name` is an expression with the following value: `body('ParseEvent')?['dimensionValues']?[2]`

**Confirm data is being saved successfully**:

1. Confirm that your **Logic App**s are running properly by going to **Overview** in the sidebar and checking the **Runs history**:
   ![runs history](https://i.ibb.co/zF0Hrns/Clean-Shot-2020-08-19-at-11-03-43.png).
2. Confirm that event data is being stored in CDS properly by checking the **Data** section of your entity inside of the [Power Apps Portal](https://make.preview.powerapps.com/). Be sure to toggle the view to `Custom fields`. Check this for both the `Audit Event` and the `Latency Event` entities.
   ![cds data](https://i.ibb.co/ZxdhMRT/Clean-Shot-2020-08-19-at-11-06-19.png)

   Note: You may not see data immediately as your Logic Apps must take time to receive events, process them, and save them to CDS.

## Edit or Run Power App

On the [Power Apps Portal](https://make.preview.powerapps.com/), navigate to **Apps** on the left sidebar, select the `D365 Fraud Protection Portal Admin App`, and click either **Edit** to modify the app in **Power Apps Studio** or click **Play** to launch the app. Note, you may have to verify and authenticate certain connections (such as that to your CDS entities for `Audit Events` and `Latency Events`).
