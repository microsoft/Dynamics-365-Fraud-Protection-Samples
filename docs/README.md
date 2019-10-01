# Microsoft Dynamics 365 Fraud Protection - API examples
## Overview

You can inform Dynamics 365 Fraud Protection of events that happen in your system. Ultimately, this knowledge helps you reduce fraud.

These documents are largely based on a sample application developed to demonstrate how you can integrate your system with Dynamics 365 Fraud Protection. In general, the documents link to actual sample application code, where possible; otherwise, code samples exist directly in the documentation.

## Contents
- [Configure the sample site](./Configure&#32;the&#32;sample&#32;site.md)
- [Authenticate and call Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- **Assess account sign up fraud**
  * [Account sign up - Approved and rejected sign up flows](./Account&#32;sign&#32;up&#32;-&#32;Approved&#32;and&#32;rejected&#32;sign&#32;up&#32;flows.md)
- **Update user accounts**
  * [Account updates - Update user profile](./Account&#32;updates&#32;-&#32;Update&#32;user&#32;profile.md)
  * [Account updates - Update credit cards](./Account&#32;updates&#32;-&#32;Update&#32;credit&#32;cards.md)
- **Assess purchase fraud**
  * [Make a purchase - Approved purchase flow](./Make&#32;a&#32;purchase&#32;-&#32;Approved&#32;purchase&#32;flow.md)
  * [Make a purchase - Handle the response](./Make&#32;a&#32;purchase&#32;-&#32;Handle&#32;the&#32;Fraud&#32;Protection&#32;purchase&#32;response.md)
  * [Make a purchase - Rejected purchase flow](./Make&#32;a&#32;purchase&#32;-&#32;Rejected&#32;purchase&#32;flow.md)
  * [Make a purchase - Existing user](./Make&#32;a&#32;purchase&#32;-&#32;Existing&#32;user.md)
  * [Make a purchase - Guest user](./Make&#32;a&#32;purchase&#32;-&#32;Guest&#32;user.md)
- **Send refunds, chargebacks, and additional fraud signals**
  * [Record a refund](./Record&#32;a&#32;refund.md)
  * [Record a chargeback](./Record&#32;a&#32;chargeback.md)
  * [Send additional fraud signals using labels](./Send&#32;additional&#32;fraud&#32;signals&#32;using&#32;labels.md)

## Product documentation
In addition to this API documentation, you can read the complimentary [product documentation for Dynamics 365 Fraud Protection](https://go.microsoft.com/fwlink/?linkid=2082391). It covers the broad set of Dynamics 365 Fraud Protection features such as the rules engine, reporting, and customer support. It also contains release notes and highlights planned, upcoming features.

## API generalities
- There are two types of API events:
  - **Assessment**: These return a risk assessment including a risk score and merchant rule decision. Examples include purchase and sign up.
  - **Non-Assessment**: These create or update data in Dynamics 365 Fraud Protection, but do not return a risk assessment. Examples include refunds and account updates.

- You cannot update assessment events. Resending the same purchase or sign up ID in a different request will result in the original assessment response being returned.

- You can update all other Dynamics 365 Fraud Protection events. Sending events to Dynamics 365 Fraud Protection 'upserts' data, meaning that it automatically creates or updates an object based on its ID. As a caller to Dynamics 365 Fraud Protection, do not be concerned if the object already exists. Of course, you should pay attention to what IDs you give to objects within events sent to Dynamics 365 Fraud Protection. Basically, event data fields not provided in requests result in the current data in Dynamics 365 Fraud Protection being preserved rather than being cleared or overwritten. You can update objects as frequently as needed (besides assessment events, as already noted).
