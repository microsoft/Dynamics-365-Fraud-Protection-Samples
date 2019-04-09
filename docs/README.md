# Microsoft Dynamics 365 Fraud Protection - API examples
## Overview

You can inform Dynamics 365 Fraud Protection of events that happen in your system. Ultimately, this knowledge helps you reduce fraud.

These documents are largely based on a sample application developed to demonstrate how you can integrate your system with Dynamics 365 Fraud Protection. In general, the documents link to actual sample application code, where possible; otherwise, code samples exist directly in the documentation.

## Contents
- [Authenticate and call Dynamics 365 Fraud Protection](./Authenticate&#32;and&#32;call&#32;Fraud&#32;Protection.md)
- **Create and update users**
  * [Create or update a user](./Create&#32;or&#32;update&#32;a&#32;user.md)
  * [Create or update a user credit card](./Create&#32;or&#32;update&#32;a&#32;user&#32;credit&#32;card.md)
- **Track purchases**
  * [Make a purchase - Approved purchase flow](./Make&#32;a&#32;purchase&#32;-&#32;Approved&#32;purchase&#32;flow.md)
  * [Make a purchase - Handle Fraud Protection purchase response](./Make&#32;a&#32;purchase&#32;-&#32;Handle&#32;the&#32;Fraud&#32;Protection&#32;purchase&#32;response.md)
  * [Make a purchase - Rejected purchase flow](./Make&#32;a&#32;purchase&#32;-&#32;Rejected&#32;purchase&#32;flow.md)
  * [Make a purchase - Existing user](./Make&#32;a&#32;purchase&#32;-&#32;Existing&#32;user.md)
  * [Make a purchase - Guest user](./Make&#32;a&#32;purchase&#32;-&#32;Guest&#32;user.md)
- **Refunds and chargebacks**
  * [Record a refund](./Record&#32;a&#32;refund.md)
  * [Record a chargeback](./Record&#32;a&#32;chargeback.md)
   
## API generalities
- You cannot update purchase events. After you send a purchase ID, you cannot send another purchase event with the same purchase ID. If you re-send the same purchase ID, an error response is returned.

- You can update all other Dynamics 365 Fraud Protection events. Sending events to Dynamics 365 Fraud Protection 'upserts' data, meaning that it automatically creates or updates an object based on its ID. As a caller to Dynamics 365 Fraud Protection, do not be concerned if the object already exists. Of course, you should pay attention to what IDs you give to objects within events sent to Dynamics 365 Fraud Protection. Basically, event data fields not provided in requests result in the current data in Dynamics 365 Fraud Protection being preserved rather than being cleared or overwritten. You can update objects as frequently as needed (besides purchase events, as already noted).
