using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using IotHub.Models;

namespace PumpMaster.Api.Services
{
    public class CosmosDbService
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public CosmosDbService(string connectionString, string databaseName, string containerName)
        {
            _cosmosClient = new CosmosClient(connectionString);
            _container = _cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task StoreTelemetryAsync(PumpTelemetry telemetry)
        {
            await _container.CreateItemAsync(telemetry, new PartitionKey(telemetry.DeviceId));
        }

        public async Task<List<PumpTelemetry>> GetTelemetryByDeviceAsync(string deviceId, int hours = 24)
        {
            var query = new QueryDefinition(
                "SELECT * FROM c WHERE c.deviceId = @deviceId AND c.timestamp >= @startTime ORDER BY c.timestamp DESC")
                .WithParameter("@deviceId", deviceId)
                .WithParameter("@startTime", DateTime.UtcNow.AddHours(-hours));

            var results = new List<PumpTelemetry>();
            using var iterator = _container.GetItemQueryIterator<PumpTelemetry>(query, requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(deviceId) });
            
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task<List<PumpTelemetry>> GetAlertsAsync(int hours = 24)
        {
            var query = new QueryDefinition(
                "SELECT * FROM c WHERE (c.temperature > 80 OR c.pressure < 10) AND c.timestamp >= @startTime ORDER BY c.timestamp DESC")
                .WithParameter("@startTime", DateTime.UtcNow.AddHours(-hours));

            var results = new List<PumpTelemetry>();
            using var iterator = _container.GetItemQueryIterator<PumpTelemetry>(query);
            
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public async Task<PumpTelemetry> GetLatestTelemetryAsync(string deviceId)
        {
            var query = new QueryDefinition(
                "SELECT TOP 1 * FROM c WHERE c.deviceId = @deviceId ORDER BY c.timestamp DESC")
                .WithParameter("@deviceId", deviceId);

            using var iterator = _container.GetItemQueryIterator<PumpTelemetry>(query, requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(deviceId) });
            
            if (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                return response.FirstOrDefault();
            }
            return null;
        }

        public async Task<List<PumpSummary>> GetPumpSummariesAsync()
        {
            var query = new QueryDefinition(@"
                SELECT 
                    c.deviceId,
                    MAX(c.timestamp) as lastSeen,
                    c.status,
                    AVG(c.temperature) as avgTemperature,
                    AVG(c.pressure) as avgPressure,
                    COUNT(1) as alertCount
                FROM c 
                WHERE c.timestamp >= DateTimeAdd('hour', -24, GetCurrentDateTime())
                GROUP BY c.deviceId, c.status
            ");

            var results = new List<PumpSummary>();
            using var iterator = _container.GetItemQueryIterator<PumpSummary>(query);
            
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }
            return results;
        }

        public void Dispose()
        {
            _cosmosClient?.Dispose();
        }
    }
}