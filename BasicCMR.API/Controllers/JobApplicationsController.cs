using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicCMR.Infrastructure.Data;
using BasicCMR.Domain.Entities;
using BasicCMR.Application.DTOs.JobApplications;
using System.Security.Claims;

namespace BasicCMR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JobApplicationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JobApplicationsController(AppDbContext context)
        {
            _context = context;
        }

        // 🧩 GET: api/jobapplications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetJobApplications()
        {
            int userId = GetUserIdFromToken();

            var applications = await _context.JobApplications
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AppliedAt)
                .Select(a => new JobApplicationDto
                {
                    Id = a.Id,
                    Position = a.Position,
                    Company = a.Company,
                    Status = a.Status,
                    AppliedAt = a.AppliedAt,
                    Notes = a.Notes,
                    JobLink = a.JobLink,
                    UserId = a.UserId
                })
                .ToListAsync();

            return Ok(applications);
        }

        // 🧩 GET: api/jobapplications/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<JobApplicationDto>> GetJobApplication(int id)
        {
            int userId = GetUserIdFromToken();

            var app = await _context.JobApplications
                .Where(a => a.Id == id && a.UserId == userId)
                .Select(a => new JobApplicationDto
                {
                    Id = a.Id,
                    Position = a.Position,
                    Company = a.Company,
                    Status = a.Status,
                    AppliedAt = a.AppliedAt,
                    Notes = a.Notes,
                    JobLink = a.JobLink,
                    UserId = a.UserId
                })
                .FirstOrDefaultAsync();

            if (app == null)
                return NotFound("Job application not found.");

            return Ok(app);
        }

        // 🟢 POST: api/jobapplications
        [HttpPost]
        public async Task<ActionResult<JobApplicationDto>> CreateJobApplication(CreateJobApplicationDto dto)
        {
            int userId = GetUserIdFromToken();

            var jobApp = new JobApplication
            {
                Position = dto.Position,
                Company = dto.Company,
                Status = dto.Status,
                Notes = dto.Notes,
                JobLink = dto.JobLink,
                UserId = userId
            };

            _context.JobApplications.Add(jobApp);
            await _context.SaveChangesAsync();

            var result = new JobApplicationDto
            {
                Id = jobApp.Id,
                Position = jobApp.Position,
                Company = jobApp.Company,
                Status = jobApp.Status,
                AppliedAt = jobApp.AppliedAt,
                Notes = jobApp.Notes,
                JobLink = jobApp.JobLink,
                UserId = jobApp.UserId
            };

            return CreatedAtAction(nameof(GetJobApplication), new { id = jobApp.Id }, result);
        }

        // 🟡 PUT: api/jobapplications/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobApplication(int id, [FromBody] UpdateJobApplicationDto dto)
        {
            int userId = GetUserIdFromToken();

            var jobApp = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (jobApp == null)
                return NotFound("Job application not found.");

            jobApp.Position = dto.Position;
            jobApp.Company = dto.Company;
            jobApp.Status = dto.Status;
            jobApp.Notes = dto.Notes;
            jobApp.JobLink = dto.JobLink;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Job application updated successfully." });
        }

        // 🗑 DELETE: api/jobapplications/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobApplication(int id)
        {
            int userId = GetUserIdFromToken();

            var jobApp = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (jobApp == null)
                return NotFound("Job application not found.");

            _context.JobApplications.Remove(jobApp);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Job application deleted successfully." });
        }

        // 🧠 Hjälpmetod – Hämta UserId från JWT
        private int GetUserIdFromToken()
        {
            var userIdClaim =
                User.FindFirst("UserId")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("No user ID found in token.");

            // ⚙️ Om claimen råkar vara ett e-postadress (felaktigt token)
            if (userIdClaim.Contains("@"))
                throw new UnauthorizedAccessException("Invalid token: UserId claim is email, not numeric ID.");

            return int.Parse(userIdClaim);
        }
    }
}