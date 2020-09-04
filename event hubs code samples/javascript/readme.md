# Sending & Receiving Events to and from Event Hubs

## Prerequisites

- Microsoft Azure Subscription
- Node.js version 8.x or later
- VSCode / an IDE
- Active Event Hubs namespace and Event Hub. [Learn how](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-create)
- Active Azure storage account and blob container
  1. [Create an Azure storage account](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal)
  2. [Create a blob container in the storage account](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-portal#create-a-container)
  3. [Get the connection string to the storage account](https://docs.microsoft.com/en-us/azure/storage/common/storage-configure-connection-string)

## Setup

Run the following command in a Terminal window pointing to the directory of this project:

```shell
npm install
```

Setup the .env file with your own connection strings, names, etc based on your own Event Hubs and Azure Storage configuration. Omit any quotations.

## Run

In one terminal window, start the script to listen to and receive the events:

```shell
node receive.js
```

In another terminal window, start the script to send the events:

```shell
node send.js
```

You should see the events logged out in the console on the receiving end.

## Additional Information

https://docs.microsoft.com/en-us/azure/event-hubs/get-started-node-send-v2
