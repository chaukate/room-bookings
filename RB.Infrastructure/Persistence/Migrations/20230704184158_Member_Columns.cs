using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RB.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Member_Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SlackUserId",
                table: "Members",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SlackUserImage",
                table: "Members",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_SlackUserId",
                table: "Members",
                column: "SlackUserId",
                unique: true,
                filter: "[SlackUserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Members_SlackUserId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "SlackUserId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "SlackUserImage",
                table: "Members");
        }
    }
}
