using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrgChartDemo.Migrations
{
    public partial class memberdemo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_MemberRank_RankId",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberRank",
                table: "MemberRank");

            migrationBuilder.RenameTable(
                name: "MemberRank",
                newName: "MemberRanks");

            migrationBuilder.AddColumn<int>(
                name: "GenderId",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RaceMemberRaceId",
                table: "Members",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberRanks",
                table: "MemberRanks",
                column: "RankId");

            migrationBuilder.CreateTable(
                name: "MemberGender",
                columns: table => new
                {
                    GenderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GenderFullName = table.Column<string>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberGender", x => x.GenderId);
                });

            migrationBuilder.CreateTable(
                name: "MemberRace",
                columns: table => new
                {
                    MemberRaceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MemberRaceFullName = table.Column<string>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRace", x => x.MemberRaceId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_GenderId",
                table: "Members",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_RaceMemberRaceId",
                table: "Members",
                column: "RaceMemberRaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_MemberGender_GenderId",
                table: "Members",
                column: "GenderId",
                principalTable: "MemberGender",
                principalColumn: "GenderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_MemberRace_RaceMemberRaceId",
                table: "Members",
                column: "RaceMemberRaceId",
                principalTable: "MemberRace",
                principalColumn: "MemberRaceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_MemberRanks_RankId",
                table: "Members",
                column: "RankId",
                principalTable: "MemberRanks",
                principalColumn: "RankId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_MemberGender_GenderId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_MemberRace_RaceMemberRaceId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_MemberRanks_RankId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "MemberGender");

            migrationBuilder.DropTable(
                name: "MemberRace");

            migrationBuilder.DropIndex(
                name: "IX_Members_GenderId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_RaceMemberRaceId",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberRanks",
                table: "MemberRanks");

            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "RaceMemberRaceId",
                table: "Members");

            migrationBuilder.RenameTable(
                name: "MemberRanks",
                newName: "MemberRank");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberRank",
                table: "MemberRank",
                column: "RankId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_MemberRank_RankId",
                table: "Members",
                column: "RankId",
                principalTable: "MemberRank",
                principalColumn: "RankId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
