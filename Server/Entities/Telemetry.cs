using Server.Entities.Common;

namespace Server.Entities
{
    public class Telemetry : BaseEntitiy
    {
        public string DeviceId { get; set; }
        public double Illuminance { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
