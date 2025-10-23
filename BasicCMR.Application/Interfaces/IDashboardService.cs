using BasicCMR.Application.DTOs.Dashboard;

namespace BasicCMR.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync(int userId);
        Task<IEnumerable<RecentJobApplicationDto>> GetRecentApplicationsAsync(int userId, int count = 5);
        Task<IEnumerable<StatusDistributionDto>> GetStatusDistributionAsync(int userId);
    }
}
