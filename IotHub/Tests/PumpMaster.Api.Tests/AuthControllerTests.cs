using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using PumpMaster.Api.Controllers;
using Xunit;

namespace PumpMaster.Api.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("YourSuperSecretKeyThatIsAtLeast32CharactersLong");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("PumpMaster.Api");
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("PumpMaster.Client");
            
            _controller = new AuthController(_mockConfiguration.Object);
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var request = new LoginRequest { Username = "admin", Password = "password123" };

            // Act
            var result = _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest { Username = "invalid", Password = "invalid" };

            // Act
            var result = _controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}