using System;

namespace BasicCMR.Application.DTOs.Dashboard
{
    public class RecentJobApplicationDto
    {
        public int Id { get; set; }
        public string Position { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime AppliedAt { get; set; }
    }
}
