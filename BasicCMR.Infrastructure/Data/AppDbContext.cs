using BasicCMR.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BasicCMR.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Tabellen för användare (DATA)
        public DbSet<User> Users { get; set; }

        // Tabellen för jobbansökningar (DATA)
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🧱 Unik constraint på e-post
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Relation: en användare har många jobbansökningar
            modelBuilder.Entity<JobApplication>()
                .HasOne(a => a.User)
                .WithMany(u => u.JobApplications)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
