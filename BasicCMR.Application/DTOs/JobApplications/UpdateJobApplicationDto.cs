namespace BasicCMR.Application.DTOs.JobApplications
{
    public class UpdateJobApplicationDto
    {
        public string Position { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string JobLink { get; set; } = string.Empty;
    }
}
