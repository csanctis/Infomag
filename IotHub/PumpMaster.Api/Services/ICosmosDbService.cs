using IotHub.Models;

namespace PumpMaster.Api.Services
{
    public interface ICosmosDbService
    {
        Task AddTelemetryAsync(PumpTelemetry telemetry);
        Task<List<PumpTelemetry>> GetTelemetryByDeviceAsync(string deviceId, int hours = 24);
        Task<List<PumpTelemetry>> GetAlertsAsync(int hours = 24);
        Task<PumpTelemetry> GetLatestTelemetryAsync(string deviceId);
        Task<List<PumpSummary>> GetPumpSummariesAsync();
        Task<List<PumpTelemetry>> GetRecentTelemetryAsync(int hours = 24);
    }
}