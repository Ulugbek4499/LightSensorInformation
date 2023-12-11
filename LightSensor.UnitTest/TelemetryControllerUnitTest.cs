using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.Controllers;
using Server.DataBase;
using Server.Entities;

namespace LightSensor.UnitTest
{
    public class TelemetryControllerUnitTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task SaveTelemetryData_IfDeviceIsNullOrEmpty_ShouldReturnBadRequest(string deviceId)
        {
            // Arrange
            var mockContext = new Mock<IApplicationDbContext>();
            var mockMediator = new Mock<IMediator>();

            var telemetryController = new TelemetryController(mockContext.Object, mockMediator.Object);
            var telemetryData = new List<TelemetryEntry> 
            { new TelemetryEntry { Time = 123456789, Illuminance = 10.0 } };

            // Set up the HTTP context with a mocked User
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "TestUserId") };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = claimsPrincipal };

            telemetryController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await telemetryController.SaveTelemetryData(deviceId, telemetryData);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid device ID", badRequestResult.Value);
        }

        [Fact]
        public async Task GetStatistics_WhenUserNotAuthenticated_ShouldReturnUnauthorized()
        {
            // Arrange
            var mockContext = new Mock<IApplicationDbContext>();
            var mockMediator = new Mock<IMediator>();

            var telemetryController = new TelemetryController(mockContext.Object, mockMediator.Object);
            var deviceId = "1";

            // Set up the HTTP context with a non-authenticated user
            var httpContext = new DefaultHttpContext { User = new ClaimsPrincipal() };

            telemetryController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await telemetryController.GetStatistics(deviceId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);

            // Assert that the status code is 401 (Unauthorized)
            Assert.Equal(StatusCodes.Status401Unauthorized, objectResult.StatusCode);

            // You can also assert other aspects of the result based on your specific requirements
        }


        [Fact]
        public async Task SaveTelemetryData_WhenUserNotAuthenticated_ShouldNotReturnUnauthorized()
        {
            // Arrange
            var mockContext = new Mock<IApplicationDbContext>();
            var mockMediator = new Mock<IMediator>();

            var telemetryController = new TelemetryController(mockContext.Object, mockMediator.Object);
            var telemetryData = new List<TelemetryEntry> { new TelemetryEntry { Time = 123456789, Illuminance = 10.0 } };
            var deviceId = "TestDevice";

            // Set up the HTTP context with a non-authenticated user
            var httpContext = new DefaultHttpContext { User = new ClaimsPrincipal() };

            telemetryController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await telemetryController.SaveTelemetryData(deviceId, telemetryData);

            // Assert
            Assert.IsNotType<UnauthorizedResult>(result);
            // You can also assert other aspects of the result based on your specific requirements
        }


        [Fact]
        public async Task SaveTelemetryData_IfInvalidTimeFormat_ShouldReturnInternalServerError()
        {
            // Arrange
            var mockContext = new Mock<IApplicationDbContext>();
            var mockMediator = new Mock<IMediator>();

            var telemetryController = new TelemetryController(mockContext.Object, mockMediator.Object);
            var telemetryData = new List<TelemetryEntry> { new TelemetryEntry { Time = -1, Illuminance = 10.0 } };
            var deviceId = "TestDevice";

            // Set up the HTTP context with a mocked User
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "TestUserId") };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = claimsPrincipal };

            telemetryController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await telemetryController.SaveTelemetryData(deviceId, telemetryData);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Internal Server Error", statusCodeResult.Value);
        }

     

      

    }
}
