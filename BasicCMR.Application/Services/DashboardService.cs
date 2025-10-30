using BasicCMR.Application.DTOs.Dashboard;
using BasicCMR.Application.DTOs.JobApplications;
using BasicCMR.Application.Interfaces;
using BasicCMR.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BasicCMR.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        // 🔹 Summering (antal ansökningar totalt)
        public async Task<DashboardSummaryDto> GetSummaryAsync(int userId)
        {
            try
            {
                var total = await _context.JobApplications
                    .CountAsync(j => j.UserId == userId);

                return new DashboardSummaryDto
                {
                    TotalApplications = total
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Fel i GetSummaryAsync: {ex.Message}");
                return new DashboardSummaryDto { TotalApplications = 0 };
            }
        }

        // 🔹 Hämtar de senaste ansökningarna
        public async Task<IEnumerable<JobApplicationDto>> GetRecentApplicationsAsync(int userId, int count)
        {
            try
            {
                var applications = await _context.JobApplications
                    .Where(j => j.UserId == userId)
                    .OrderByDescending(j => j.AppliedAt)
                    .Take(count)
                    .Select(j => new JobApplicationDto
                    {
                        Id = j.Id,
                        Position = j.Position,
                        Company = j.Company,
                        Status = j.Status,
                        AppliedAt = j.AppliedAt,
                        Notes = j.Notes,
                        JobLink = j.JobLink,
                        UserId = j.UserId
                    })
                    .ToListAsync();

                return applications ?? new List<JobApplicationDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Fel i GetRecentApplicationsAsync: {ex.Message}");
                return new List<JobApplicationDto>();
            }
        }

        // 🔹 Fördelning av statusar (Offer / Pending / Rejected)
        public async Task<StatusDistributionDto> GetStatusDistributionAsync(int userId)
        {
            try
            {
                var all = await _context.JobApplications
                    .Where(j => j.UserId == userId)
                    .ToListAsync();

                if (all == null || !all.Any())
                {
                    return new StatusDistributionDto
                    {
                        Accepted = 0,
                        Pending = 0,
                        Rejected = 0
                    };
                }

                return new StatusDistributionDto
                {
                    // “Offer” räknas som accepterad
                    Accepted = all.Count(a => a.Status != null && a.Status.Equals("Offer", StringComparison.OrdinalIgnoreCase)),

                    // “Applied” + “Interview” räknas som pågående
                    Pending = all.Count(a =>
                        a.Status != null && (
                            a.Status.Equals("Applied", StringComparison.OrdinalIgnoreCase) ||
                            a.Status.Equals("Interview", StringComparison.OrdinalIgnoreCase)
                        )
                    ),

                    // “Rejected” räknas som nekad
                    Rejected = all.Count(a =>
                        a.Status != null && a.Status.Equals("Rejected", StringComparison.OrdinalIgnoreCase)
                    )
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Fel i GetStatusDistributionAsync: {ex.Message}");
                return new StatusDistributionDto { Accepted = 0, Pending = 0, Rejected = 0 };
            }
        }
    }
}
