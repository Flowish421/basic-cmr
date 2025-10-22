using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicCMR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJobApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobLink",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobLink",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "JobApplications");
        }
    }
}
