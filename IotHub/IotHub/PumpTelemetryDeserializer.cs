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

        /*
         Key functionalities
            Device-to-cloud communication: Devices can securely send telemetry data to the cloud for processing, analysis, and storage. 
            Device provisioning and management: It helps register and provision devices, and provides capabilities for over-the-air updates. 
            Security: IoT hubs offer secure authentication and encrypted data transfer to protect against unauthorized access and data tampering. 
            Scalability: They are designed to handle millions of devices and messages, with varying service tiers to meet different needs. 
        How it works
            A physical device, like a sensor or appliance, sends data to the IoT hub. 
            The IoT hub authenticates the device and encrypts the data. 
            The hub then routes the data to other cloud-based services for processing, such as real-time analysis or long-term storage. 
            The application on the cloud can send commands back to the devices via the hub to perform actions. 
        Common use cases
            Industrial automation: Monitor machine performance and predict maintenance needs.
         */
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
                                    /*
                                        This is the simplest method for basic data materialization and aggregations. 
                                        Locate Event Hub: In the Azure portal, navigate to your Event Hubs instance.
                                        Process Data: Under "Features," select "Process Data."
                                        Materialize in Cosmos DB: Choose "Materialize data in Cosmos DB" and provide a name for your Stream Analytics job.
                                        Configure Input & Output: Configure the Event Hub as the input and your Cosmos DB account, database, and container as the output.
                                        Define Transformations (Optional): If needed, you can use the editor to filter, project, or aggregate data before writing to Cosmos DB.
                                        Start Job: Save and start the Stream Analytics job.

                                     */
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