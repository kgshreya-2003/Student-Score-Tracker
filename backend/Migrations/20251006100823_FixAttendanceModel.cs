using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentScoreTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class FixAttendanceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Attendance");

            migrationBuilder.AddColumn<bool>(
                name: "IsPresent",
                table: "Attendance",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPresent",
                table: "Attendance");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Attendance",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
