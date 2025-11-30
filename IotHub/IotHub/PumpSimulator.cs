using System;
using System.Threading.Tasks;
using IotHub.Models;
using IotHub.Services;

namespace IotHub
{
    public class PumpSimulator
    {
        private readonly IoTHubService _iotHubService;
        private readonly Random _random = new Random();

        public PumpSimulator(string connectionString)
        {
            _iotHubService = new IoTHubService(connectionString);
        }

        public async Task StartSimulationAsync(string deviceId)
        {
            await _iotHubService.InitializeAsync(deviceId);

            while (true)
            {
                var telemetry = GenerateTelemetry(deviceId);
                await _iotHubService.SendTelemetryAsync(telemetry);
                await Task.Delay(5000); // Send every 5 seconds
            }
        }

        private PumpTelemetry GenerateTelemetry(string deviceId)
        {
            return new PumpTelemetry
            {
                DeviceId = deviceId,
                Timestamp = DateTime.UtcNow,
                Temperature = 20 + _random.NextDouble() * 60,
                Pressure = 10 + _random.NextDouble() * 40,
                FlowRate = 50 + _random.NextDouble() * 100,
                Vibration = _random.NextDouble() * 5,
                Status = _random.Next(100) > 5 ? "Normal" : "Warning",
                Location = new Location
                {
                    Latitude = -26.6500 + (_random.NextDouble() - 0.5) * 0.1,
                    Longitude = 153.0667 + (_random.NextDouble() - 0.5) * 0.1
                }
            };
        }
    }
}