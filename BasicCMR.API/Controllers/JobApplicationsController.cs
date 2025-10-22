using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicCMR.Infrastructure.Data;
using BasicCMR.Domain.Entities;
using BasicCMR.API.DTOs.JobApplications;
using System.Security.Claims;


namespace BasicCMR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // kräver inloggning (JWT)
    public class JobApplicationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JobApplicationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/JobApplications 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetJobApplications()
        {
            int userId = GetUserIdFromToken();

            var applications = await _context.JobApplications
                .Where(a => a.UserId == userId)
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

        // GET: api/JobApplications/{id} Med ID
        [HttpGet("{id}")]
        public async Task<ActionResult<JobApplicationDto>> GetJobApplication(int id)
        {
            int userId = GetUserIdFromToken();

            var application = await _context.JobApplications
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

            if (application == null)
                return NotFound();

            return Ok(application);
        }

        // POST: api/JobApplications
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

        // 🟡 PUT: api/JobApplications/{id} Med id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobApplication(int id, UpdateJobApplicationDto dto)
        {
            int userId = GetUserIdFromToken();

            var jobApp = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (jobApp == null)
                return NotFound();

            // 🔧 Uppdatera endast de fält som skickats in
            if (!string.IsNullOrEmpty(dto.Position)) jobApp.Position = dto.Position;
            if (!string.IsNullOrEmpty(dto.Company)) jobApp.Company = dto.Company;
            if (!string.IsNullOrEmpty(dto.Status)) jobApp.Status = dto.Status;
            if (!string.IsNullOrEmpty(dto.Notes)) jobApp.Notes = dto.Notes;
            if (!string.IsNullOrEmpty(dto.JobLink)) jobApp.JobLink = dto.JobLink;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/JobApplications/{id} Med ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobApplication(int id)
        {
            int userId = GetUserIdFromToken();

            var jobApp = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (jobApp == null)
                return NotFound();

            _context.JobApplications.Remove(jobApp);
            await _context.SaveChangesAsync();

            return NoContent();
            Console.WriteLine($"Trying to delete JobApplication ID={id} for User={userId}");

        }

        // Det är en hjälpmetod för att hämta UserId från JWT-tokenen
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("No user ID found in token.");

            return int.Parse(userIdClaim.Value);
        }
    } 
    

}
