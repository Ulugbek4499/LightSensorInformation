namespace LightSensorInformation.Entities
{
    public class LightSensor
    {
        public string Units { get; } = "Lux";
        public double Resolution { get; } = 0.5;
        public TimeSpan MeasurementFrequency { get; } = TimeSpan.FromMinutes(15);
        public TimeSpan SendDataFrequency { get; } = TimeSpan.FromHours(1);

        // Simulated data
        private Random random = new Random();

        // Method to simulate sensor data
        public SensorData SimulateSensorData()
        {
            // Simulate illumination increase in the first half of the day and decrease in the second half
            double illumination = random.Next(80, 200); // Adjust the range based on your scenario
            DateTime timestamp = DateTime.UtcNow;

            // Depending on the time of day, adjust illumination
            if (timestamp.Hour < 12)
            {
                illumination += random.NextDouble() * 20; // Simulate increase
            }
            else
            {
                illumination -= random.NextDouble() * 20; // Simulate decrease
            }

            // Round illumination to the specified resolution
            illumination = Math.Round(illumination / Resolution) * Resolution;

            return new SensorData { Illuminance = illumination, Timestamp = (long)timestamp.Subtract(new DateTime(1970, 1, 1)).TotalSeconds };
        }
    }
}
