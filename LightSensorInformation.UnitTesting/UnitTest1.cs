using Cqrs.Hosts;
using System.Net;
using System.Text;

namespace LightSensorInformation.UnitTesting
{
    public class TelemetryControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public TelemetryControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task SaveTelemetryData_ValidData_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var deviceId = "yourDeviceId";
            var telemetryData = new { /* your telemetry data here */ };

            // Act
            var response = await client.PostAsync($"/devices/{deviceId}/telemetry",
                new StringContent(JsonSerializer.Serialize(telemetryData), Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetStatistics_ValidDeviceId_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var deviceId = "yourDeviceId";

            // Act
            var response = await client.GetAsync($"/devices/{deviceId}/statistics");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}