using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrgChartDemo.Migrations
{
    public partial class AppStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppStatusId",
                table: "Members",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationStatuses",
                columns: table => new
                {
                    AppStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StatusName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationStatuses", x => x.AppStatusId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_AppStatusId",
                table: "Members",
                column: "AppStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_ApplicationStatuses_AppStatusId",
                table: "Members",
                column: "AppStatusId",
                principalTable: "ApplicationStatuses",
                principalColumn: "AppStatusId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_ApplicationStatuses_AppStatusId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "ApplicationStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Members_AppStatusId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "AppStatusId",
                table: "Members");
        }
    }
}
