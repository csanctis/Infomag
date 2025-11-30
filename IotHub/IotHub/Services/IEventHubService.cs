using System;
using System.Threading.Tasks;
using IotHub.Models;

namespace IotHub.Services
{
    public interface IEventHubService : IDisposable
    {
        Task SendTelemetryAsync(PumpTelemetry telemetry);
    }
}