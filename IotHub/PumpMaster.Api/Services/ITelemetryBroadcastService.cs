using IotHub.Models;

namespace PumpMaster.Api.Services
{
    public interface ITelemetryBroadcastService
    {
        Task BroadcastTelemetryAsync(PumpTelemetry telemetry);
        Task BroadcastAlertAsync(PumpTelemetry alert);
        Task BroadcastToDeviceGroupAsync(string deviceId, PumpTelemetry telemetry);
    }
}