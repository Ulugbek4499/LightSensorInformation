using System.ComponentModel.DataAnnotations;
using Server.Entities.Common;

namespace Server.Entities;

public class Telemetry : BaseEntitiy
{
    [Required(ErrorMessage = "DeviceId is required")]
    public string DeviceId { get; set; }

    [Required(ErrorMessage = "Illuminance is required")]
    public double Illuminance { get; set; }

    [Required(ErrorMessage = "Timestamp is required")]
    public DateTime Timestamp { get; set; }
}
