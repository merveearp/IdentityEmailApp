using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityEmailApp.Migrations
{
    /// <inheritdoc />
    public partial class AddConversationIdToMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "Messages",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Messages");
        }
    }
}
