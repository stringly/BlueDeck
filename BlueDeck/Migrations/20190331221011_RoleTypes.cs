using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueDeck.Migrations
{
    public partial class RoleTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_RoleType_RoleTypeId",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleType",
                table: "RoleType");

            migrationBuilder.RenameTable(
                name: "RoleType",
                newName: "RoleTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleTypes",
                table: "RoleTypes",
                column: "RoleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_RoleTypes_RoleTypeId",
                table: "Roles",
                column: "RoleTypeId",
                principalTable: "RoleTypes",
                principalColumn: "RoleTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_RoleTypes_RoleTypeId",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleTypes",
                table: "RoleTypes");

            migrationBuilder.RenameTable(
                name: "RoleTypes",
                newName: "RoleType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleType",
                table: "RoleType",
                column: "RoleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_RoleType_RoleTypeId",
                table: "Roles",
                column: "RoleTypeId",
                principalTable: "RoleType",
                principalColumn: "RoleTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
