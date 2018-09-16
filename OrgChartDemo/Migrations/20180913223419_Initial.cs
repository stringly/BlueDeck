using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrgChartDemo.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    ComponentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentComponentComponentId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Acronym = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_Components_Components_ParentComponentComponentId",
                        column: x => x.ParentComponentComponentId,
                        principalTable: "Components",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberRank",
                columns: table => new
                {
                    RankId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RankFullName = table.Column<string>(nullable: true),
                    RankShort = table.Column<string>(nullable: true),
                    PayGrade = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRank", x => x.RankId);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    PostionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentComponentComponentId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    IsUnique = table.Column<bool>(nullable: false),
                    JobTitle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.PostionId);
                    table.ForeignKey(
                        name: "FK_Positions_Components_ParentComponentComponentId",
                        column: x => x.ParentComponentComponentId,
                        principalTable: "Components",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RankId = table.Column<int>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    IdNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PositionPostionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_Members_Positions_PositionPostionId",
                        column: x => x.PositionPostionId,
                        principalTable: "Positions",
                        principalColumn: "PostionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Members_MemberRank_RankId",
                        column: x => x.RankId,
                        principalTable: "MemberRank",
                        principalColumn: "RankId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Components_ParentComponentComponentId",
                table: "Components",
                column: "ParentComponentComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_PositionPostionId",
                table: "Members",
                column: "PositionPostionId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_RankId",
                table: "Members",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_ParentComponentComponentId",
                table: "Positions",
                column: "ParentComponentComponentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "MemberRank");

            migrationBuilder.DropTable(
                name: "Components");
        }
    }
}
