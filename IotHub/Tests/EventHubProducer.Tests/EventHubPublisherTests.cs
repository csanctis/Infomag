using EventHubProducer;
using EventHubProducer.Models;
using Xunit;

namespace EventHubProducer.Tests
{
    public class EventHubPublisherTests
    {
        [Fact]
        public void EventHubPublisher_Constructor_ThrowsOnNullConnectionString()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new EventHubPublisher(null, "test-hub"));
        }

        [Fact]
        public void EventHubPublisher_Constructor_ThrowsOnNullEventHubName()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new EventHubPublisher("test-connection", null));
        }

        [Fact]
        public void PumpTelemetry_Model_HasRequiredProperties()
        {
            // Arrange & Act
            var telemetry = new PumpTelemetry
            {
                DeviceId = "test-device",
                Temperature = 25.0,
                Pressure = 15.0
            };

            // Assert
            Assert.NotNull(telemetry.Id);
            Assert.Equal("test-device", telemetry.DeviceId);
            Assert.Equal(25.0, telemetry.Temperature);
            Assert.Equal(15.0, telemetry.Pressure);
        }
    }
}