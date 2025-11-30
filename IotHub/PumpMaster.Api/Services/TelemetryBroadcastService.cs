using Microsoft.AspNetCore.SignalR;
using PumpMaster.Api.Hubs;
using IotHub.Models;

namespace PumpMaster.Api.Services
{
    public class TelemetryBroadcastService : ITelemetryBroadcastService
    {
        private readonly IHubContext<TelemetryHub> _hubContext;

        public TelemetryBroadcastService(IHubContext<TelemetryHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task BroadcastTelemetryAsync(PumpTelemetry telemetry)
        {
            await _hubContext.Clients.All.SendAsync("TelemetryUpdate", telemetry);
        }

        public async Task BroadcastAlertAsync(PumpTelemetry alert)
        {
            await _hubContext.Clients.All.SendAsync("AlertUpdate", alert);
        }

        public async Task BroadcastToDeviceGroupAsync(string deviceId, PumpTelemetry telemetry)
        {
            await _hubContext.Clients.Group($"device-{deviceId}").SendAsync("DeviceTelemetryUpdate", telemetry);
        }
    }
}