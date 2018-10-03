using Microsoft.EntityFrameworkCore.Migrations;

namespace OrgChartDemo.Migrations
{
#pragma warning disable 1591
    public partial class PositionIsManager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsManager",
                table: "Positions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsManager",
                table: "Positions");
        }
    }
#pragma warning restore 1591
}
