namespace BasicCMR.API.DTOs.JobApplications
{
    public class CreateJobApplicationDto
    {
        public string Position { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Status { get; set; } = "Applied";
        public string? Notes { get; set; }
        public string? JobLink { get; set; }
    }
}
