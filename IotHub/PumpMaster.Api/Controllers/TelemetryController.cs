using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PumpMaster.Api.Services;
using IotHub.Models;

namespace PumpMaster.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TelemetryController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;

        public TelemetryController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet("{deviceId}")]
        public async Task<ActionResult<List<PumpTelemetry>>> GetTelemetry(string deviceId, [FromQuery] int hours = 24)
        {
            var telemetry = await _cosmosDbService.GetTelemetryByDeviceAsync(deviceId, hours);
            return Ok(telemetry);
        }

        [HttpGet("{deviceId}/latest")]
        public async Task<ActionResult<PumpTelemetry>> GetLatestTelemetry(string deviceId)
        {
            var telemetry = await _cosmosDbService.GetLatestTelemetryAsync(deviceId);
            if (telemetry == null)
                return NotFound();
            return Ok(telemetry);
        }

        [HttpGet("alerts")]
        public async Task<ActionResult<List<PumpTelemetry>>> GetAlerts([FromQuery] int hours = 24)
        {
            var alerts = await _cosmosDbService.GetAlertsAsync(hours);
            return Ok(alerts);
        }

        [HttpGet("summaries")]
        public async Task<ActionResult<List<PumpSummary>>> GetSummaries()
        {
            var summaries = await _cosmosDbService.GetPumpSummariesAsync();
            return Ok(summaries);
        }

        [HttpGet]
        public async Task<ActionResult<List<PumpTelemetry>>> GetAllTelemetry([FromQuery] int hours = 24)
        {
            var telemetry = await _cosmosDbService.GetRecentTelemetryAsync(hours);
            return Ok(telemetry);
        }
    }
}