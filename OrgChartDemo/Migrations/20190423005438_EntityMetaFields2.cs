using Microsoft.EntityFrameworkCore.Migrations;

namespace OrgChartDemo.Migrations
{
    public partial class EntityMetaFields2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Members_CreatorId",
                table: "Members");

            migrationBuilder.CreateIndex(
                name: "IX_Members_CreatorId",
                table: "Members",
                column: "CreatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Members_CreatorId",
                table: "Members");

            migrationBuilder.CreateIndex(
                name: "IX_Members_CreatorId",
                table: "Members",
                column: "CreatorId",
                unique: true,
                filter: "[CreatorId] IS NOT NULL");
        }
    }
}
