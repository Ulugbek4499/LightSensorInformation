using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Entities
{
    public class Telemetry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public double Illuminance { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
