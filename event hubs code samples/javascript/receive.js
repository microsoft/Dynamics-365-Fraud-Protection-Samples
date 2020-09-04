const { EventHubConsumerClient } = require('@azure/event-hubs');
const { ContainerClient } = require('@azure/storage-blob');
const {
  BlobCheckpointStore,
} = require('@azure/eventhubs-checkpointstore-blob');
require('dotenv').config();

async function main() {
  // Create a blob container client and a blob checkpoint store using the client.
  const containerClient = new ContainerClient(
    process.env.STORAGE_CONNECTION_STRING,
    process.env.STORAGE_CONTAINER_NAME
  );
  const checkpointStore = new BlobCheckpointStore(containerClient);

  // Create a consumer client for the event hub by specifying the checkpoint store.
  const consumerClient = new EventHubConsumerClient(
    process.env.CONSUMER_GROUP,
    process.env.EVENT_HUBS_CONNECTION_STRING,
    process.env.EVENT_HUB_NAME,
    checkpointStore
  );
  console.log('Now listening to events...');

  // Subscribe to the events, and specify handlers for processing the events and errors.
  const subscription = consumerClient.subscribe({
    processEvents: async (events, context) => {
      for (const event of events) {
        console.log('↼ Received the following event ⇀');
        // console.log(JSON.stringify(event));
        console.log(event);
      }
      // Update the checkpoint.
      await context.updateCheckpoint(events[events.length - 1]);
    },

    processError: async (err, context) => {
      console.log(`Error : ${err}`);
    },
  });

  /* Clean Up Code (unsubscribe) */
  // await subscription.close();
  // await consumerClient.close();
}

main().catch((err) => {
  console.log('Error occurred: ', err);
});
