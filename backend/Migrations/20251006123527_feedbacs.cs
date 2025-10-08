using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentScoreTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class feedbacs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FeedbackText",
                table: "Feedbacks",
                newName: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Feedbacks",
                newName: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Feedbacks",
                table: "Feedbacks",
                newName: "FeedbackText");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Feedbacks",
                newName: "CreatedAt");
        }
    }
}
