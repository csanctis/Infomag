using IotHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Devices;
using PumpMaster.Api.Services;
using System;

namespace PumpMaster.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RealtimeController : ControllerBase
    {
        private readonly TelemetryBroadcastService _broadcastService;
        private readonly CosmosDbService _cosmosDbService;
        private readonly Random _random = new Random();

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
                DeviceId = "pump-" + _random.NextDouble(),
                Timestamp = DateTime.UtcNow,
                Temperature = 20 + _random.NextDouble() * 60,
                Pressure = 10 + _random.NextDouble() * 40,
                FlowRate = 50 + _random.NextDouble() * 100,
                Vibration = _random.NextDouble() * 5,
                Status = _random.Next(100) < 50 ? "Normal" : "Warning",
                Location = new Location
                {
                    Latitude = -26.6500 + (_random.NextDouble() - 0.5) * 0.1,
                    Longitude = 153.0667 + (_random.NextDouble() - 0.5) * 0.1
                }
            };

            await _cosmosDbService.AddTelemetryAsync(telemetry);
            await _broadcastService.BroadcastTelemetryAsync(telemetry);
            return Ok(telemetry);
        }
    }
}