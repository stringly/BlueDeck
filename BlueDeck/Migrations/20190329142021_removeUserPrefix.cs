using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueDeck.Migrations
{
    public partial class removeUserPrefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Members_MemberId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_RoleType_RoleTypeId",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "Roles");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleTypeId",
                table: "Roles",
                newName: "IX_Roles_RoleTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_MemberId",
                table: "Roles",
                newName: "IX_Roles_MemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Members_MemberId",
                table: "Roles",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_RoleType_RoleTypeId",
                table: "Roles",
                column: "RoleTypeId",
                principalTable: "RoleType",
                principalColumn: "RoleTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Members_MemberId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_RoleType_RoleTypeId",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "UserRoles");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_RoleTypeId",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_MemberId",
                table: "UserRoles",
                newName: "IX_UserRoles_MemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Members_MemberId",
                table: "UserRoles",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_RoleType_RoleTypeId",
                table: "UserRoles",
                column: "RoleTypeId",
                principalTable: "RoleType",
                principalColumn: "RoleTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
