using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrgChartDemo.Migrations
{
    public partial class UserRolesAndRoleTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "UserRoles");

            migrationBuilder.AddColumn<int>(
                name: "RoleTypeId",
                table: "UserRoles",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserRoleType",
                columns: table => new
                {
                    RoleTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleType", x => x.RoleTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleTypeId",
                table: "UserRoles",
                column: "RoleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_UserRoleType_RoleTypeId",
                table: "UserRoles",
                column: "RoleTypeId",
                principalTable: "UserRoleType",
                principalColumn: "RoleTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_UserRoleType_RoleTypeId",
                table: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserRoleType");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_RoleTypeId",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "RoleTypeId",
                table: "UserRoles");

            migrationBuilder.AddColumn<int>(
                name: "RoleName",
                table: "UserRoles",
                nullable: false,
                defaultValue: 0);
        }
    }
}
