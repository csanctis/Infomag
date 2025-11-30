using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.StreamAnalytics;
using Microsoft.Azure.StreamAnalytics.Serialization;
using Newtonsoft.Json;
using IotHub.Models;
using IotHub.Services;

namespace IotHub
{
    public class PumpTelemetryDeserializer : StreamDeserializer<PumpTelemetry>
    {
        private StreamingDiagnostics streamingDiagnostics;
        private EventHubService eventHubService;

        public override void Initialize(StreamingContext streamingContext)
        {
            this.streamingDiagnostics = streamingContext.Diagnostics;
            
            var connectionString = Environment.GetEnvironmentVariable("EVENTHUB_CONNECTION_STRING");
            var eventHubName = Environment.GetEnvironmentVariable("EVENTHUB_NAME");
            
            if (!string.IsNullOrEmpty(connectionString) && !string.IsNullOrEmpty(eventHubName))
            {
                eventHubService = new EventHubService(connectionString, eventHubName);
            }
        }

        public override IEnumerable<PumpTelemetry> Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    PumpTelemetry telemetry = null;
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        try
                        {
                            telemetry = JsonConvert.DeserializeObject<PumpTelemetry>(line);
                        }
                        catch (JsonException ex)
                        {
                            streamingDiagnostics?.WriteError("JSON deserialization failed", $"Invalid JSON: {line}, Error: {ex.Message}");
                        }
                    }
                    if (telemetry != null && !string.IsNullOrEmpty(telemetry.DeviceId))
                    {
                        // Publish to Event Hub (fire and forget)
                        if (eventHubService != null)
                        {
                            _ = Task.Run(async () =>
                            {
                                try
                                {
                                    await eventHubService.SendTelemetryAsync(telemetry);
                                }
                                catch (Exception ex)
                                {
                                    streamingDiagnostics?.WriteError("EventHub publish failed", $"DeviceId: {telemetry.DeviceId}, Error: {ex.Message}");
                                }
                            });
                        }
                        
                        yield return telemetry;
                    }
                    line = sr.ReadLine();
                }
            }
        }
    }
}