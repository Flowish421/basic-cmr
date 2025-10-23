using BasicCMR.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BasicCMR.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var userId = GetUserId();
            var summary = await _dashboardService.GetSummaryAsync(userId);
            return Ok(summary);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecent([FromQuery] int count = 5)
        {
            var userId = GetUserId();
            var recent = await _dashboardService.GetRecentApplicationsAsync(userId, count);
            return Ok(recent);
        }

        [HttpGet("status-distribution")]
        public async Task<IActionResult> GetStatusDistribution()
        {
            var userId = GetUserId();
            var data = await _dashboardService.GetStatusDistributionAsync(userId);
            return Ok(data);
        }
    }
}
