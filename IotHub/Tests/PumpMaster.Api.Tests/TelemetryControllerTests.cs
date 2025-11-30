using Microsoft.AspNetCore.Mvc;
using Moq;
using PumpMaster.Api.Controllers;
using PumpMaster.Api.Services;
using IotHub.Models;
using Xunit;

namespace PumpMaster.Api.Tests
{
    public class TelemetryControllerTests
    {
        [Fact]
        public async Task GetAllTelemetry_ReturnsOkResult()
        {
            // Arrange
            var mockService = new Mock<ICosmosDbService>();
            mockService.Setup(s => s.GetRecentTelemetryAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<PumpTelemetry>());
            var controller = new TelemetryController(mockService.Object);

            // Act
            var result = await controller.GetAllTelemetry();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetLatestTelemetry_WithNullResult_ReturnsNotFound()
        {
            // Arrange
            var mockService = new Mock<ICosmosDbService>();
            mockService.Setup(s => s.GetLatestTelemetryAsync(It.IsAny<string>()))
                .ReturnsAsync((PumpTelemetry)null);
            var controller = new TelemetryController(mockService.Object);

            // Act
            var result = await controller.GetLatestTelemetry("test");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetTelemetry_ReturnsOkResult()
        {
            // Arrange
            var mockService = new Mock<ICosmosDbService>();
            mockService.Setup(s => s.GetTelemetryByDeviceAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(new List<PumpTelemetry>());
            var controller = new TelemetryController(mockService.Object);

            // Act
            var result = await controller.GetTelemetry("test");

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}