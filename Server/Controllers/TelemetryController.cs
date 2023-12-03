using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.DataBase;
using Server.Entities;
using Server.Notifications;

namespace Server.Controllers
{
    [ApiController]
    [Route("devices")]
    public class TelemetryController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public TelemetryController(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        [HttpPost("{deviceId}/telemetry")]
        public async Task<IActionResult> SaveTelemetryData(string deviceId, List<TelemetryEntry> telemetryData)
        {
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    return BadRequest("Invalid device ID");
                }

                foreach (var entry in telemetryData)
                {
                    if (entry.Time <= 0 || entry.Illuminance < 0)
                    {
                        continue; 
                    }

                    var telemetry = new Telemetry
                    {
                        DeviceId = deviceId,
                        Illuminance = entry.Illuminance,
                        Timestamp = DateTimeOffset.FromUnixTimeSeconds(entry.Time).UtcDateTime
                    };

                    _context.Telementries.Add(telemetry);
                    await _mediator.Publish(new SaveTelemetryNotification(
                                            telemetry.DeviceId, telemetry.Illuminance, telemetry.Timestamp));
                }

                await _context.SaveChangesAsync();
                    
                return Ok("Telemetry data saved successfully");
            }
            catch (Exception ex)
            {
                await _mediator.Publish(new SaveTelemetryExcetionNotification(deviceId, ex));

                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpGet("{deviceId}/statistics")]
        public async Task<IActionResult> GetStatistics(string deviceId)
        {
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    return BadRequest("Invalid device ID");
                }

                var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
                var statistics = _context.Telementries
                         .Where(t => t.DeviceId == deviceId && t.Timestamp >= thirtyDaysAgo)
                         .GroupBy(t => t.Timestamp.Date) 
                         .Select(group => new
                            {
                                 Date = group.Key,
                                 MaxIlluminance = group.Max(t => t.Illuminance)
                            })
                         .OrderBy(stat => stat.Date)
                         .ToList();

                await _mediator.Publish(new GetStatisticsNotification());

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                await _mediator.Publish(new GetStatisticsExceptionNotification(deviceId, ex));

                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}