using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using IotHub.Models;

namespace IotHub.Services
{
    public class IoTHubService
    {
        private readonly string _connectionString;
        private DeviceClient _deviceClient;

        public IoTHubService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task InitializeAsync(string deviceId)
        {
            _deviceClient = DeviceClient.CreateFromConnectionString(_connectionString, deviceId);
            await _deviceClient.OpenAsync();
        }

        public async Task SendTelemetryAsync(PumpTelemetry telemetry)
        {
            var json = JsonConvert.SerializeObject(telemetry);
            var message = new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(json))
            {
                ContentType = "application/json",
                ContentEncoding = "utf-8"
            };

            // Add properties for Stream Analytics routing
            message.Properties.Add("deviceType", "pump");
            message.Properties.Add("messageType", GetMessageType(telemetry));

            await _deviceClient.SendEventAsync(message);
        }

        private string GetMessageType(PumpTelemetry telemetry)
        {
            if (telemetry.Temperature > 80 || telemetry.Pressure < 10)
                return "alert";
            return "telemetry";
        }

        public void Dispose()
        {
            _deviceClient?.Dispose();
        }
    }
}