using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;
using EventHubProducer.Models;

namespace EventHubProducer
{
    public class EventHubPublisher : IEventHubPublisher
    {
        private readonly EventHubProducerClient producerClient;
        private bool disposed;

        public EventHubPublisher(string connectionString, string eventHubName)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException("connectionString is required", nameof(connectionString));
            if (string.IsNullOrWhiteSpace(eventHubName)) throw new ArgumentException("eventHubName is required", nameof(eventHubName));

            this.producerClient = new EventHubProducerClient(connectionString, eventHubName);
        }

        public async Task SendTelemetryAsync(PumpTelemetry telemetry)
        {
            if (telemetry == null) throw new ArgumentNullException(nameof(telemetry));

            string json = JsonConvert.SerializeObject(telemetry);
            using EventDataBatch batch = await producerClient.CreateBatchAsync().ConfigureAwait(false);

            var eventData = new EventData(Encoding.UTF8.GetBytes(json))
            {
                ContentType = "application/json"
            };

            if (!batch.TryAdd(eventData))
            {
                throw new InvalidOperationException("Telemetry payload too large to send to Event Hub.");
            }

            await producerClient.SendAsync(batch).ConfigureAwait(false);
        }

        public void Dispose()
        {
            if (!disposed)
            {
                try
                {
                    producerClient?.DisposeAsync().AsTask().GetAwaiter().GetResult();
                }
                catch
                {
                    // swallow on dispose to avoid throwing during cleanup
                }
                disposed = true;
            }
        }
    }
}