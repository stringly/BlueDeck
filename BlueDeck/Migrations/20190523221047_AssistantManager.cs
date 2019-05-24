using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueDeck.Migrations
{
    public partial class AssistantManager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAssistantManager",
                table: "Positions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAssistantManager",
                table: "Positions");
        }
    }
}
