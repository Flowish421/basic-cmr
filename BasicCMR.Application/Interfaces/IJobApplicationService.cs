using BasicCMR.Application.DTOs.JobApplications;


namespace BasicCMR.Application.Interfaces
{
    public interface IJobApplicationService
    {
        Task<IEnumerable<JobApplicationDto>> GetAllAsync(int userId);
        Task<JobApplicationDto> GetByIdAsync(int id, int userId);
        Task<JobApplicationDto> CreateAsync(CreateJobApplicationDto dto, int userId);
        Task<JobApplicationDto> UpdateAsync(int id, UpdateJobApplicationDto dto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}
