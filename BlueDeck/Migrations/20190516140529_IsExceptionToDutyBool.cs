using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueDeck.Migrations
{
    public partial class IsExceptionToDutyBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsExceptionToNormalDuty",
                table: "DutyStatuses",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IsExceptionToNormalDuty",
                table: "DutyStatuses",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
