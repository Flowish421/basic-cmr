using BasicCMR.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BasicCMR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // ✅ Kombinerad dashboard-endpoint
        // GET: api/dashboard
        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                // 🔐 Försök hitta userId i olika möjliga claim-namn
                var userIdClaim =
                    User.FindFirst("UserId")?.Value ??
                    User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                    User.FindFirst("sub")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    Console.WriteLine("❌ Ingen giltig UserId-claim hittades i JWT-token.");
                    return Unauthorized(new { message = "Token saknar giltig användaridentifierare." });
                }

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    Console.WriteLine($"❌ Ogiltigt UserId-format i token: {userIdClaim}");
                    return BadRequest(new { message = "Ogiltigt tokenformat för UserId." });
                }

                // 🔹 Hämta dashboarddata via servicen
                var summary = await _dashboardService.GetSummaryAsync(userId);
                var recent = await _dashboardService.GetRecentApplicationsAsync(userId, 5);
                var status = await _dashboardService.GetStatusDistributionAsync(userId);

                // 🔹 Returnera resultat till frontend
                return Ok(new
                {
                    totalApplications = summary.TotalApplications,
                    accepted = status.Accepted,
                    pending = status.Pending,
                    rejected = status.Rejected,
                    recentApplications = recent
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ FEL i GetDashboard(): {ex.Message}");
                return StatusCode(500, new { message = "Internt serverfel i DashboardController", error = ex.Message });
            }
        }

        // 🧩 GET: api/dashboard/summary
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var summary = await _dashboardService.GetSummaryAsync(userId);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ FEL i GetSummary(): {ex.Message}");
                return StatusCode(500, new { message = "Serverfel vid hämtning av sammanfattning", error = ex.Message });
            }
        }

        // 🧩 GET: api/dashboard/recent
        [HttpGet("recent")]
        public async Task<IActionResult> GetRecent([FromQuery] int count = 5)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var recent = await _dashboardService.GetRecentApplicationsAsync(userId, count);
                return Ok(recent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ FEL i GetRecent(): {ex.Message}");
                return StatusCode(500, new { message = "Serverfel vid hämtning av senaste ansökningar", error = ex.Message });
            }
        }

        // 🧩 GET: api/dashboard/status
        [HttpGet("status")]
        public async Task<IActionResult> GetStatusDistribution()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var dist = await _dashboardService.GetStatusDistributionAsync(userId);
                return Ok(dist);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ FEL i GetStatusDistribution(): {ex.Message}");
                return StatusCode(500, new { message = "Serverfel vid hämtning av statusfördelning", error = ex.Message });
            }
        }

        // 🔧 Hjälpmetod – hanterar olika claim-namn
        private int GetUserIdFromToken()
        {
            var userIdClaim =
                User.FindFirst("UserId")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("Token saknar giltig användaridentifierare.");

            if (!int.TryParse(userIdClaim, out int userId))
                throw new Exception($"Ogiltigt tokenvärde för UserId: {userIdClaim}");

            return userId;
        }
    }
}
