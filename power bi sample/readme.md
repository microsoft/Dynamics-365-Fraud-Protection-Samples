# Dynamics 365 Fraud Protection Event Tracing Power BI Sample Report

## Overview

This is a template for a Power BI report with interactive graphs for Dynamics 365 Fraud Protection **Latency Event** data.

## Prerequisites

- [Azure subscription](https://azure.microsoft.com/en-us/features/azure-portal/)
- [Event Hub](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-create)
- [Dynamics 365 Fraud Protection event tracing subscription](https://docs.microsoft.com/en-us/dynamics365/fraud-protection/event-tracing) for **Latency Events** connected to your Event Hub
- Access to [Common Data Service](https://docs.microsoft.com/en-us/power-platform/admin/pricing-billing-skus) (CDS)
- [Power BI Subscription](https://powerbi.microsoft.com/en-us/)
- [Power BI Desktop](https://powerbi.microsoft.com/en-us/desktop/)

## Common Data Service (CDS) setup

To use this Power BI template out of the box, your `Latency Event` data must be stored in Common Data Service (CDS) with the same field names as the ones referenced in the template.

1. Download and save the [Dynamics365FraudProtectionEntities.zip](https://github.com/microsoft/Dynamics-365-Fraud-Protection-Samples/raw/master/power%20bi%20sample/Dynamics365FraudProtectionEntities.zip) file to your computer.
2. In the [Power Apps Portal](https://make.preview.powerapps.com/), navigate to the **Solutions** page in the left navigation.
3. Click **Import**, and then click **Choose File**
4. Select `Dynamics365FraudProtectionEntities.zip` to load the file.
5. Click **Next**, and then click **Import**.
6. Upon completion, you will have imported the following:

- `Audit Event` Common Data Service (CDS) Entity
- `Latency Event` Common Data Service (CDS) Entity

The entities are required for the functionality of the Power BI report, because Fraud Protection event data is stored to these entities as new records via **Common Data Service (CDS)**.

## Setup Logic Apps to Save Event Data to Common Data Service

We must setup two **Logic Apps** to save **Audit Event** and **Latency Event** data from your Fraud Protection portal to CDS as they arrive in **Event Hubs**.

> CDS setup with your event data only needs to be done _once_ and can be used across **Power BI**, **Power Apps**, **Power Automate**, etc. If you have previously set this up (e.g. You went through the Fraud Protection Power App sample setup), then you do not need to set this up again.

Start with the [Logic App Templates](https://github.com/microsoft/Dynamics-365-Fraud-Protection-Samples/tree/master/logic%20app%20templates) to quickly setup the base steps required for our two **Logic App** flows - one for **Audit Events**, and one for **Latency Events**.

**Audit Events Logic App**:

1. Start with the **Audit Events** Logic App template.
2. Add a **New Action** (after `ParseEvent` on the list) from **Common Data Service (CDS)** for **Create a new record**.
   ![create new record](https://i.ibb.co/dmsDcdB/Clean-Shot-2020-08-24-at-11-30-14.png).
3. Setup the fields _exactly_ as below. Click **Add new parameter** to add new fields:  
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

## Setup report from template

1. Download and save the [d365-fraud-protection-latency-report-template.pbit](https://github.com/microsoft/Dynamics-365-Fraud-Protection-Samples/raw/master/power%20bi%20sample/d365-fraud-protection-latency-report-template.pbit) file to your computer.
2. Double click the file to load it into **Power BI Desktop**.
3. On the modal that opens, enter your [CDS environment URL](https://docs.microsoft.com/en-us/powerapps/maker/common-data-service/data-platform-powerbi-connector#finding-your-common-data-service-environment-url) as the `cds_environment_connection_url`.

You should now be able to navigate to different pages of the report to view latency data across different API endpoints and timestamps.
