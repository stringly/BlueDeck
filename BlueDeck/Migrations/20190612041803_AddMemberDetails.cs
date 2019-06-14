using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueDeck.Migrations
{
    public partial class AddMemberDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LDAPName",
                table: "Members",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HireDate",
                table: "Members",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OrgPositionNumber",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayrollID",
                table: "Members",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HireDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "OrgPositionNumber",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PayrollID",
                table: "Members");

            migrationBuilder.AlterColumn<string>(
                name: "LDAPName",
                table: "Members",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
