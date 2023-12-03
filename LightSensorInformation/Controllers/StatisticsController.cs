using Microsoft.AspNetCore.Mvc;

namespace LightSensorInformation.Controllers
{

   /* [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        // Assuming you have a service to handle statistics data
        private readonly StatisticsService statisticsService;

        public StatisticsController(StatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        // GET api/statistics/{deviceId}
        [HttpGet("{deviceId}/statistics")]
        public IActionResult GetStatistics(string deviceId)
        {
            try
            {
                var statistics = statisticsService.GetMaxIlluminanceStatistics(deviceId);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal Server Error");
            }
        }
    }*/
}
