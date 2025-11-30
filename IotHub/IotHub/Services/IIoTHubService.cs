using System;
using System.Threading.Tasks;
using IotHub.Models;

namespace IotHub.Services
{
    public interface IIoTHubService : IDisposable
    {
        Task InitializeAsync(string deviceId);
        Task SendTelemetryAsync(PumpTelemetry telemetry);
    }
}