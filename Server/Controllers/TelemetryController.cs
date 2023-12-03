using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DataBase;
using Server.Entities;

namespace Server.Controllers
{
    [ApiController]
    [Route("devices")]
    public class TelemetryController : ControllerBase
    {
        private readonly ILogger<TelemetryController> _logger;
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public TelemetryController(ILogger<TelemetryController> logger, IApplicationDbContext context, IMediator mediator)
        {
            _logger = logger;
            _context = context;
            _mediator = mediator;
        }

        [HttpPost("{deviceId}/telemetry")]
        public async Task<IActionResult> SaveTelemetryData(string deviceId, List<TelemetryEntry> telemetryData)
        {
            try
            {
                // Validate deviceId
                if (string.IsNullOrEmpty(deviceId))
                {
                    return BadRequest("Invalid device ID");
                }

                // Validate and save telemetry data
                foreach (var entry in telemetryData)
                {
                    if (entry.Time <= 0 || entry.Illuminance < 0)
                    {
                        _logger.LogWarning($"Invalid telemetry data received for device {deviceId}: {entry}");
                        continue; // Skip invalid data
                    }

                    var telemetry = new Telemetry
                    {
                        DeviceId = deviceId,
                        Illuminance = entry.Illuminance,
                        Timestamp = DateTimeOffset.FromUnixTimeSeconds(entry.Time).UtcDateTime
                    };

                    _context.Telementries.Add(telemetry);
                }

                await _context.SaveChangesAsync();
                return Ok("Telemetry data saved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving telemetry data for device {deviceId}: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpGet("{deviceId}/statistics")]
        public IActionResult GetStatistics(string deviceId)
        {
            try
            {
                // Validate deviceId
                if (string.IsNullOrEmpty(deviceId))
                {
                    return BadRequest("Invalid device ID");
                }

                // Retrieve maximum illuminance values for the last thirty days
                var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
                var statistics = _context.Telementries
                         .Where(t => t.DeviceId == deviceId && t.Timestamp >= thirtyDaysAgo)
                         .GroupBy(t => t.Timestamp.Date) // Use Date property to extract the date part
                         .Select(group => new
                            {
                                 Date = group.Key,
                                 MaxIlluminance = group.Max(t => t.Illuminance)
                            })
                         .OrderBy(stat => stat.Date)
                         .ToList();



                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving statistics for device {deviceId}: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}

       /* [HttpPost("{deviceId}/telemetry")]
        public async Task<IActionResult> SaveTelemetryDataAsync(string deviceId, List<TelemetryEntry> telemetryData)
        {
            try
            {
                // Validate deviceId
                if (string.IsNullOrEmpty(deviceId))
                {
                    return BadRequest("Invalid device ID");
                }

                // Validate and save telemetry data
                foreach (var entry in telemetryData)
                {
                    if (entry.Time <= 0 || entry.Illuminance < 0)
                    {
                        _logger.LogWarning($"Invalid telemetry data received for device {deviceId}: {entry}");
                        continue; // Skip invalid data
                    }

                    var telemetry = new Telemetry
                    {
                        DeviceId = deviceId,
                        Illuminance = entry.Illuminance,
                        Timestamp = DateTimeOffset.FromUnixTimeSeconds(entry.Time).UtcDateTime
                    };

                    _context.Telementries.Add(telemetry);
                }

                await _context.SaveChangesAsync();

                return Ok("Telemetry data saved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving telemetry data for device {deviceId}: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }*/