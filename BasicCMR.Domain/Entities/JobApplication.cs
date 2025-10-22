namespace BasicCMR.Domain.Entities
{
    public class JobApplication
    {
        public int Id { get; set; }
        public string Position { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Status { get; set; } = "Applied";
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
        public string? JobLink { get; set; }

        // Relation till användare
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
