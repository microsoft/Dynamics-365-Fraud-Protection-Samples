# Dynamics 365 Fraud Protection Event Tracing Logic App Templates

## Overview

These are a set of Logic App templates that work specifically with Dynamics 365 Fraud Protection events received inside Event Hubs for getting started quickly with working with event data.

## Prerequisites

- Azure subscription
- Event Hub Namespace
- Event Hub(s)
- Dynamics 365 Fraud Protection event tracing subscriptions connected to your Event Hub(s)

For help setting up the above, check the documentation in **Reference Links** in the following section.

> **Note**:  
> It is highly advised to have a _separate_ **Event Hub** for each event _type_ subscription. For example, you have a `audit-events` **Event Hub** setup with a Fraud Protection Event Tracing subscription sending **Audit Events** and another separate `latency-events` **Event Hub** setup with another subscription sending **Latency Events**. They can be under the same namespace.

## Reference Links

[Dynamics 365 Fraud Protection Event Tracing Documentation](https://docs.microsoft.com/en-us/dynamics365/fraud-protection/event-tracing)

## Getting started with the templates

**Audit Events**:

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmicrosoft%2FDynamics-365-Fraud-Protection-Samples%2Fmaster%2Flogic%2520app%2520templates%2Faudit-events-template.json)

**Latency Events**:

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmicrosoft%2FDynamics-365-Fraud-Protection-Samples%2Fmaster%2Flogic%2520app%2520templates%2Flatency-events-template.json)

1. Open one of the above quick deploy buttons in a **New Tab** to begin. Each deployment is associated with a specific _type_ of event, so ensure that the event tracing subscriptions you set up match with the template types you use here.
   For example, you can have an **Event Hub** called `audit-events` that is connected to the Fraud Protection portal subscription sending **Audit Events**, and use the **Audit Events** Logic App template above.
2. Select your **Subscription** and **Resource Group** using the dropdown.
3. Set the **Logic App Name** to one of your choosing.
4. Set the **Event Hubs Namespace Connection String** to the connection string for your **Event Hub Namespace**. To get this string, see [here](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-get-connection-string). Note, this is the connection string for the **Event Hubs Namespace** not the individual **Event Hub**.
5. Set the **Event Hub Name** to the name of your individual **Event Hub** (eg. `audit-events`). Note, this **Event Hub** must be receiving events of the same type as the template type you chose.
6. Agree to terms and conditions.
7. Select **Purchase**.
8. Your new **Logic App** should be setup and deployed successfully.
9. Navigate to the **Logic App Designer** page and begin developing your **Logic App**.

## Next Steps

1. Select **New Step** in the **Logic App Designer** page to start working with the event data.
2. For example, you can send an email with attributes from incoming **Audit Event**s such as **entityName** as shown here. Note: you may have to press **See More** when loading in dynamic content. </br>  
   ![send email example](https://i.ibb.co/Y8LXj1C/example.png)

[For more information on Logic Apps, go here.](https://docs.microsoft.com/en-us/azure/logic-apps/)
