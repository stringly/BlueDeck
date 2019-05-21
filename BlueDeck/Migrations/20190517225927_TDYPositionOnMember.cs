using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueDeck.Migrations
{
    public partial class TDYPositionOnMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TempPositionId",
                table: "Members",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_TempPositionId",
                table: "Members",
                column: "TempPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Positions_TempPositionId",
                table: "Members",
                column: "TempPositionId",
                principalTable: "Positions",
                principalColumn: "PositionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Positions_TempPositionId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_TempPositionId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "TempPositionId",
                table: "Members");
        }
    }
}
