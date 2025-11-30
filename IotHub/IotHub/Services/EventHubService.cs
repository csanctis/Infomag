using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Newtonsoft.Json;
using IotHub.Models;

namespace IotHub.Services
{
    public class EventHubService : IDisposable
    {
        private readonly EventHubProducerClient _producerClient;

        public EventHubService(string connectionString, string eventHubName)
        {
            _producerClient = new EventHubProducerClient(connectionString, eventHubName);
        }

        public async Task SendTelemetryAsync(PumpTelemetry telemetry)
        {
            var json = JsonConvert.SerializeObject(telemetry);
            var eventData = new EventData(Encoding.UTF8.GetBytes(json));
            
            eventData.Properties.Add("deviceId", telemetry.DeviceId);
            eventData.Properties.Add("messageType", "telemetry");
            
            using var eventBatch = await _producerClient.CreateBatchAsync();
            eventBatch.TryAdd(eventData);
            
            await _producerClient.SendAsync(eventBatch);
        }

        public void Dispose()
        {
            _producerClient?.DisposeAsync().AsTask().Wait();
        }
    }
}