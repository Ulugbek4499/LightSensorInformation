namespace LightSensorInformation.Entities
{
    public class TelemetryData
    {
        public string DeviceId { get; set; }
        public double Illuminance { get; set; }
        public long Timestamp { get; set; }
    }
}
