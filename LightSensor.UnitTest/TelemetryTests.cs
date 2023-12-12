using System.ComponentModel.DataAnnotations;
using Server.Entities;

namespace LightSensor.UnitTest
{
    public class TelemetryTests
    {
        [Fact]
        public void Telemetry_AllPropertiesValidatedSuccessfully()
        {
            // Arrange
            var telemetry = new Telemetry
            {
                DeviceId = "TestDevice",
                Illuminance = 10.0,
                Timestamp = DateTime.UtcNow
            };

            // Act
            var validationErrors = ValidateModelAndGetErrors(telemetry);

            // Assert
            Assert.Empty(validationErrors);
        }

        [Fact]
        public void Telemetry_DeviceIdIsRequired()
        {
            // Arrange
            var telemetry = new Telemetry
            {
                Illuminance = 10.0,
                Timestamp = DateTime.UtcNow
            };

            // Act
            var validationErrors = ValidateModelAndGetErrors(telemetry);

            // Assert
            Assert.NotEmpty(validationErrors);
            Assert.Contains("DeviceId is required", validationErrors);
        }

        private string ValidateModelAndGetErrors(object model)
        {
            var validationContext = new ValidationContext(model, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);

            if (!isValid)
            {
                return string.Join(", ", validationResults.Select(r => r.ErrorMessage));
            }

            return string.Empty;
        }
    }
}
