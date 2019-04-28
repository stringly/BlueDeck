using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrgChartDemo.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    ComponentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentComponentId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Acronym = table.Column<string>(nullable: true),
                    LineupPosition = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_Components_Components_ParentComponentId",
                        column: x => x.ParentComponentId,
                        principalTable: "Components",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DutyStatus",
                columns: table => new
                {
                    DutyStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DutyStatusName = table.Column<string>(nullable: true),
                    HasPolicePower = table.Column<bool>(nullable: false),
                    Abbreviation = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DutyStatus", x => x.DutyStatusId);
                });

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

            migrationBuilder.CreateTable(
                name: "MemberRanks",
                columns: table => new
                {
                    RankId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RankFullName = table.Column<string>(nullable: true),
                    RankShort = table.Column<string>(nullable: true),
                    PayGrade = table.Column<string>(nullable: true),
                    IsSworn = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRanks", x => x.RankId);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumberTypes",
                columns: table => new
                {
                    PhoneNumberTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PhoneNumberTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumberTypes", x => x.PhoneNumberTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RoleType",
                columns: table => new
                {
                    RoleTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleType", x => x.RoleTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    PositionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentComponentId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsUnique = table.Column<bool>(nullable: false),
                    JobTitle = table.Column<string>(nullable: true),
                    IsManager = table.Column<bool>(nullable: false),
                    Callsign = table.Column<string>(nullable: true),
                    LineupPosition = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.PositionId);
                    table.ForeignKey(
                        name: "FK_Positions_Components_ParentComponentId",
                        column: x => x.ParentComponentId,
                        principalTable: "Components",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RankId = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    IdNumber = table.Column<string>(nullable: true),
                    GenderId = table.Column<int>(nullable: false),
                    RaceId = table.Column<int>(nullable: false),
                    DutyStatusId = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    LDAPName = table.Column<string>(nullable: true),
                    PositionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_Members_DutyStatus_DutyStatusId",
                        column: x => x.DutyStatusId,
                        principalTable: "DutyStatus",
                        principalColumn: "DutyStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_MemberGender_GenderId",
                        column: x => x.GenderId,
                        principalTable: "MemberGender",
                        principalColumn: "GenderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "PositionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_MemberRace_RaceId",
                        column: x => x.RaceId,
                        principalTable: "MemberRace",
                        principalColumn: "MemberRaceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_MemberRanks_RankId",
                        column: x => x.RankId,
                        principalTable: "MemberRanks",
                        principalColumn: "RankId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactNumbers",
                columns: table => new
                {
                    MemberContactNumberId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TypePhoneNumberTypeId = table.Column<int>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    MemberId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactNumbers", x => x.MemberContactNumberId);
                    table.ForeignKey(
                        name: "FK_ContactNumbers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactNumbers_PhoneNumberTypes_TypePhoneNumberTypeId",
                        column: x => x.TypePhoneNumberTypeId,
                        principalTable: "PhoneNumberTypes",
                        principalColumn: "PhoneNumberTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleTypeId = table.Column<int>(nullable: true),
                    MemberId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_UserRoles_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_RoleType_RoleTypeId",
                        column: x => x.RoleTypeId,
                        principalTable: "RoleType",
                        principalColumn: "RoleTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Components_ParentComponentId",
                table: "Components",
                column: "ParentComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactNumbers_MemberId",
                table: "ContactNumbers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactNumbers_TypePhoneNumberTypeId",
                table: "ContactNumbers",
                column: "TypePhoneNumberTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_DutyStatusId",
                table: "Members",
                column: "DutyStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_GenderId",
                table: "Members",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_PositionId",
                table: "Members",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_RaceId",
                table: "Members",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_RankId",
                table: "Members",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_ParentComponentId",
                table: "Positions",
                column: "ParentComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_MemberId",
                table: "UserRoles",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleTypeId",
                table: "UserRoles",
                column: "RoleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactNumbers");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "PhoneNumberTypes");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "RoleType");

            migrationBuilder.DropTable(
                name: "DutyStatus");

            migrationBuilder.DropTable(
                name: "MemberGender");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "MemberRace");

            migrationBuilder.DropTable(
                name: "MemberRanks");

            migrationBuilder.DropTable(
                name: "Components");
        }
    }
}
