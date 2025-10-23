using BasicCMR.Application.DTOs.Dashboard;
using BasicCMR.Application.Interfaces;
using BasicCMR.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BasicCMR.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public DashboardService(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync(int userId)
        {
            string cacheKey = $"dashboard_summary_{userId}";

            if (_cache.TryGetValue(cacheKey, out DashboardSummaryDto cachedSummary))
                return cachedSummary;

            var query = _context.JobApplications.Where(j => j.UserId == userId);

            var summary = new DashboardSummaryDto
            {
                TotalApplications = await query.CountAsync(),
                AppliedCount = await query.CountAsync(j => j.Status == "Applied"),
                InterviewCount = await query.CountAsync(j => j.Status == "Interview"),
                OfferCount = await query.CountAsync(j => j.Status == "Offer"),
                RejectedCount = await query.CountAsync(j => j.Status == "Rejected")
            };

            _cache.Set(cacheKey, summary, TimeSpan.FromSeconds(30));
            return summary;
        }

        public async Task<IEnumerable<RecentJobApplicationDto>> GetRecentApplicationsAsync(int userId, int count = 5)
        {
            string cacheKey = $"dashboard_recent_{userId}_{count}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<RecentJobApplicationDto> cachedRecent))
                return cachedRecent;

            var recent = await _context.JobApplications
                .Where(j => j.UserId == userId)
                .OrderByDescending(j => j.AppliedAt)
                .Take(count)
                .Select(j => new RecentJobApplicationDto
                {
                    Id = j.Id,
                    Position = j.Position,
                    Company = j.Company,
                    Status = j.Status,
                    AppliedAt = j.AppliedAt
                })
                .ToListAsync();

            _cache.Set(cacheKey, recent, TimeSpan.FromSeconds(30));
            return recent;
        }

        public async Task<IEnumerable<StatusDistributionDto>> GetStatusDistributionAsync(int userId)
        {
            string cacheKey = $"dashboard_status_{userId}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<StatusDistributionDto> cachedDist))
                return cachedDist;

            var query = _context.JobApplications.Where(j => j.UserId == userId);
            var total = await query.CountAsync();

            var distribution = await query
                .GroupBy(j => j.Status)
                .Select(g => new StatusDistributionDto
                {
                    Status = g.Key,
                    Count = g.Count(),
                    Percentage = total == 0 ? 0 : Math.Round((double)g.Count() / total * 100, 2)
                })
                .ToListAsync();

            _cache.Set(cacheKey, distribution, TimeSpan.FromSeconds(30));
            return distribution;
        }
    }
}
