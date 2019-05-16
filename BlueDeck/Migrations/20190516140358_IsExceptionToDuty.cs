using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueDeck.Migrations
{
    public partial class IsExceptionToDuty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PayGrade",
                table: "Ranks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MemberRaceFullName",
                table: "Races",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GenderFullName",
                table: "Genders",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DutyStatusName",
                table: "DutyStatuses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsExceptionToNormalDuty",
                table: "DutyStatuses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "StatusName",
                table: "ApplicationStatuses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExceptionToNormalDuty",
                table: "DutyStatuses");

            migrationBuilder.AlterColumn<string>(
                name: "PayGrade",
                table: "Ranks",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "MemberRaceFullName",
                table: "Races",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "GenderFullName",
                table: "Genders",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "DutyStatusName",
                table: "DutyStatuses",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "StatusName",
                table: "ApplicationStatuses",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
