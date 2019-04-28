using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueDeck.Migrations
{
    public partial class removeMemberPrefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_DutyStatus_DutyStatusId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_MemberGender_GenderId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_MemberRace_RaceId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_MemberRanks_RankId",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberRanks",
                table: "MemberRanks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberRace",
                table: "MemberRace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberGender",
                table: "MemberGender");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DutyStatus",
                table: "DutyStatus");

            migrationBuilder.RenameTable(
                name: "MemberRanks",
                newName: "Ranks");

            migrationBuilder.RenameTable(
                name: "MemberRace",
                newName: "Races");

            migrationBuilder.RenameTable(
                name: "MemberGender",
                newName: "Genders");

            migrationBuilder.RenameTable(
                name: "DutyStatus",
                newName: "DutyStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ranks",
                table: "Ranks",
                column: "RankId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Races",
                table: "Races",
                column: "MemberRaceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genders",
                table: "Genders",
                column: "GenderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DutyStatuses",
                table: "DutyStatuses",
                column: "DutyStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_DutyStatuses_DutyStatusId",
                table: "Members",
                column: "DutyStatusId",
                principalTable: "DutyStatuses",
                principalColumn: "DutyStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Genders_GenderId",
                table: "Members",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "GenderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Races_RaceId",
                table: "Members",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "MemberRaceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Ranks_RankId",
                table: "Members",
                column: "RankId",
                principalTable: "Ranks",
                principalColumn: "RankId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_DutyStatuses_DutyStatusId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Genders_GenderId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Races_RaceId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Ranks_RankId",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ranks",
                table: "Ranks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Races",
                table: "Races");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genders",
                table: "Genders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DutyStatuses",
                table: "DutyStatuses");

            migrationBuilder.RenameTable(
                name: "Ranks",
                newName: "MemberRanks");

            migrationBuilder.RenameTable(
                name: "Races",
                newName: "MemberRace");

            migrationBuilder.RenameTable(
                name: "Genders",
                newName: "MemberGender");

            migrationBuilder.RenameTable(
                name: "DutyStatuses",
                newName: "DutyStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberRanks",
                table: "MemberRanks",
                column: "RankId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberRace",
                table: "MemberRace",
                column: "MemberRaceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberGender",
                table: "MemberGender",
                column: "GenderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DutyStatus",
                table: "DutyStatus",
                column: "DutyStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_DutyStatus_DutyStatusId",
                table: "Members",
                column: "DutyStatusId",
                principalTable: "DutyStatus",
                principalColumn: "DutyStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_MemberGender_GenderId",
                table: "Members",
                column: "GenderId",
                principalTable: "MemberGender",
                principalColumn: "GenderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_MemberRace_RaceId",
                table: "Members",
                column: "RaceId",
                principalTable: "MemberRace",
                principalColumn: "MemberRaceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_MemberRanks_RankId",
                table: "Members",
                column: "RankId",
                principalTable: "MemberRanks",
                principalColumn: "RankId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
