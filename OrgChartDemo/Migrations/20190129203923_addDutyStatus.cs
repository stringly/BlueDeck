using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrgChartDemo.Migrations
{
    public partial class addDutyStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DutyStatusId",
                table: "Members",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DutyStatus",
                columns: table => new
                {
                    DutyStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DutyStatusName = table.Column<string>(nullable: true),
                    HasPolicePower = table.Column<bool>(nullable: false),
                    Abbreviation = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DutyStatus", x => x.DutyStatusId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_DutyStatusId",
                table: "Members",
                column: "DutyStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_DutyStatus_DutyStatusId",
                table: "Members",
                column: "DutyStatusId",
                principalTable: "DutyStatus",
                principalColumn: "DutyStatusId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_DutyStatus_DutyStatusId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "DutyStatus");

            migrationBuilder.DropIndex(
                name: "IX_Members_DutyStatusId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "DutyStatusId",
                table: "Members");
        }
    }
}
