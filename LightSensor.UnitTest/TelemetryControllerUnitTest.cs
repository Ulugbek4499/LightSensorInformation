using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.Controllers;
using Server.DataBase;
using Server.Entities;

namespace LightSensor.UnitTest;

public class TelemetryControllerUnitTest
{
    private readonly Mock<IApplicationDbContext> mockContext;
    private readonly Mock<IMediator> mockMediator;

    public TelemetryControllerUnitTest()
    {
        mockContext = new Mock<IApplicationDbContext>();
        mockMediator = new Mock<IMediator>();
    }

    [Fact]
    public async Task SaveTelemetryData_Success()
    {
        // Arrange
        mockContext.Setup(c => c.Telementries.Add(It.IsAny<Telemetry>())).Verifiable();
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1); 

        var telemetryController = new TelemetryController(mockContext.Object, mockMediator.Object);
        var telemetryData = new List<TelemetryEntry>
        {
         new TelemetryEntry
          {
              Time = 123456,
              Illuminance = 10.0
           }
        };
        var deviceId = "TestDevice";

        // Act
        var result = await telemetryController.SaveTelemetryData(deviceId, telemetryData);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);

        var okObjectResult = (OkObjectResult)result;

        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal("Telemetry data saved successfully", okObjectResult.Value);
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task SaveTelemetryData_Fail_WhenDeviceIdIsNullOrEmpty(string deviceId)
    {
        // Arrange

        var telemetryController = new TelemetryController(mockContext.Object, mockMediator.Object);
        var telemetryData = new List<TelemetryEntry> 
        { 
            new TelemetryEntry 
            {
                Time = 123456789, 
                Illuminance = 10.0 
            }
        };

        // Act
        var result = await telemetryController.SaveTelemetryData(deviceId, telemetryData);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid device ID", badRequestResult.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetStatistics_Fail_WhenDeviceIdIsNullOrEmpty(string deviceId)
    {
        // Arrange
        var telemetryController = new TelemetryController(mockContext.Object, mockMediator.Object);

        // Act
        var result = await telemetryController.GetStatistics(deviceId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid device ID", badRequestResult.Value);
    }
}
