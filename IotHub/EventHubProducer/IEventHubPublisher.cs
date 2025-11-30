using System;
using System.Threading.Tasks;
using EventHubProducer.Models;

namespace EventHubProducer
{
    public interface IEventHubPublisher : IDisposable
    {
        Task SendTelemetryAsync(PumpTelemetry telemetry);
    }
}