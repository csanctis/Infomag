using Microsoft.AspNetCore.Mvc;
using PumpMaster.Api.Services;
using IotHub.Models;

namespace PumpMaster.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RealtimeController : ControllerBase
    {
        private readonly TelemetryBroadcastService _broadcastService;
        private readonly CosmosDbService _cosmosDbService;

        public RealtimeController(TelemetryBroadcastService broadcastService, CosmosDbService cosmosDbService)
        {
            _broadcastService = broadcastService;
            _cosmosDbService = cosmosDbService;
        }

        [HttpPost("broadcast")]
        public async Task<IActionResult> BroadcastTelemetry([FromBody] PumpTelemetry telemetry)
        {
            await _broadcastService.BroadcastTelemetryAsync(telemetry);
            return Ok();
        }

        [HttpPost("simulate")]
        public async Task<IActionResult> SimulateTelemetry()
        {
            var telemetry = new PumpTelemetry
            {
                DeviceId = "pump-001",
                Timestamp = DateTime.UtcNow,
                Temperature = 75.5,
                Pressure = 15.2,
                FlowRate = 120.0,
                Vibration = 2.1,
                Status = "Normal",
                Location = new Location { Latitude = -26.6500, Longitude = 153.0667 }
            };

            await _broadcastService.BroadcastTelemetryAsync(telemetry);
            return Ok(telemetry);
        }
    }
}