# Dynamics 365 Fraud Protection Event Tracing Logic App Templates

## Overview

These are a set of Logic App templates that work specifically with Dynamics 365 Fraud Protection events received inside Event Hubs for getting started quickly with working with event data.

## Prerequisites

- Azure subscription
- Event Hub(s)
- Dynamics 365 Fraud Protection event tracing subscriptions connected to your Event Hub(s)

For help setting up the above, check the documentation in **Reference Links** below.

## Reference Links

[Dynamics 365 Fraud Protection Event Tracing Documentation](https://docs.microsoft.com/en-us/dynamics365/fraud-protection/event-tracing)

## Getting started with the templates

**Audit Events**:

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmicrosoft%2FDynamics-365-Fraud-Protection-Samples%2Fmaster%2Flogic%2520app%2520templates%2Faudit-events-template.json)

**Latency Events**:

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmicrosoft%2FDynamics-365-Fraud-Protection-Samples%2Fmaster%2Flogic%2520app%2520templates%2Flatency-events-template.json)

1. Click one of the above quick deploy buttons to begin. Each deployment is associated with a specific type of event, so ensure that the event tracing subscriptions you set up match with the template types you use here.
2. Select your **Subscription** and **Resource Group** using the dropdown.
3. Set the **Logic App Name** to one of your choosing.
4. Agree to terms and conditions.
5. Select **Purchase**.
6. Your new **Logic App** should be setup and deployed successfully.
7. Navigate to the **Logic App Designer** page.
8. Expand the **When events are available in Event Hubs** card and press the X next to input for **Event Hub name**. A dropdown should appear that allows you to select your Event Hub that is receiving events.  
   </br>
   ![event hub name](https://i.ibb.co/DkhMH5q/eventhubname.png)

## Next Steps

1. Select **New Step** in the **Logic App Designer** page to start working with the event data.
2. For example, you can send an email with attributes from incoming **Audit Event**s such as **entityName** as shown here. Note: you may have to press **See More** when loading in dynamic content. </br>  
   ![send email example](https://i.ibb.co/Y8LXj1C/example.png)
