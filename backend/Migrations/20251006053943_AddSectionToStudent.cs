using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentScoreTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSectionToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Section",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Section",
                table: "Students");
        }
    }
}
