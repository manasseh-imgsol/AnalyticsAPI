using AnalyticsAPI.Analytics.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsAPI.Analytics
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : Controller
    {
        private readonly AnalyticsService _analyticsService;
        public AnalyticsController(AnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet]
        public async  Task<IActionResult> GetRequests()
        {
            var requests = await _analyticsService.GetRequestLogs();
            return Ok(requests);
        }
    }
}
