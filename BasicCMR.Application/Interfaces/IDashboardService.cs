using BasicCMR.Application.DTOs.Dashboard;
using BasicCMR.Application.DTOs.JobApplications;

namespace BasicCMR.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync(int userId);
        Task<IEnumerable<JobApplicationDto>> GetRecentApplicationsAsync(int userId, int count);
        Task<StatusDistributionDto> GetStatusDistributionAsync(int userId);
    }
}
