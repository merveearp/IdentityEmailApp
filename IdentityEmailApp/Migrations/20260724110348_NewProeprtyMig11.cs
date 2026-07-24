using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityEmailApp.Migrations
{
    /// <inheritdoc />
    public partial class NewProeprtyMig11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "Messages",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSpam",
                table: "Messages",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsStarred",
                table: "Messages",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDraft",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsSpam",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsStarred",
                table: "Messages");
        }
    }
}
