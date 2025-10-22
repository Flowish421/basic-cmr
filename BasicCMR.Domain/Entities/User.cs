namespace BasicCMR.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relation: en användare kan ha flera jobbansökningar
        public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    }
}
