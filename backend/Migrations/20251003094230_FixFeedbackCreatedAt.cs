using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentScoreTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class FixFeedbackCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Users_TeacherId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_TeacherId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Feedbacks",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Feedbacks",
                newName: "FeedbackText");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FeedbackText",
                table: "Feedbacks",
                newName: "Comment");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Feedbacks",
                newName: "Date");

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_TeacherId",
                table: "Feedbacks",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Users_TeacherId",
                table: "Feedbacks",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
