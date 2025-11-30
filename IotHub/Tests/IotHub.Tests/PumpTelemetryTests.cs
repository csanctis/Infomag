using IotHub.Models;
using Xunit;

namespace IotHub.Tests
{
    public class PumpTelemetryTests
    {
        [Fact]
        public void PumpTelemetry_DefaultConstructor_GeneratesId()
        {
            // Act
            var telemetry = new PumpTelemetry();

            // Assert
            Assert.NotNull(telemetry.Id);
            Assert.NotEmpty(telemetry.Id);
        }

        [Fact]
        public void PumpTelemetry_SetProperties_ReturnsCorrectValues()
        {
            // Arrange
            var deviceId = "pump-123";
            var temperature = 25.5;
            var location = new Location { Latitude = -26.6500, Longitude = 153.0667 };

            // Act
            var telemetry = new PumpTelemetry
            {
                DeviceId = deviceId,
                Temperature = temperature,
                Location = location
            };

            // Assert
            Assert.Equal(deviceId, telemetry.DeviceId);
            Assert.Equal(temperature, telemetry.Temperature);
            Assert.Equal(location.Latitude, telemetry.Location.Latitude);
            Assert.Equal(location.Longitude, telemetry.Location.Longitude);
        }
    }
}