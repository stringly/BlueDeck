using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueDeck.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationStatuses",
                columns: table => new
                {
                    AppStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StatusName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationStatuses", x => x.AppStatusId);
                });

            migrationBuilder.CreateTable(
                name: "DutyStatuses",
                columns: table => new
                {
                    DutyStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DutyStatusName = table.Column<string>(nullable: false),
                    HasPolicePower = table.Column<bool>(nullable: false),
                    Abbreviation = table.Column<string>(nullable: false),
                    IsExceptionToNormalDuty = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DutyStatuses", x => x.DutyStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    GenderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GenderFullName = table.Column<string>(nullable: false),
                    Abbreviation = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.GenderId);
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
                name: "Races",
                columns: table => new
                {
                    MemberRaceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MemberRaceFullName = table.Column<string>(nullable: false),
                    Abbreviation = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.MemberRaceId);
                });

            migrationBuilder.CreateTable(
                name: "Ranks",
                columns: table => new
                {
                    RankId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RankFullName = table.Column<string>(nullable: true),
                    RankShort = table.Column<string>(nullable: true),
                    PayGrade = table.Column<string>(nullable: false),
                    IsSworn = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranks", x => x.RankId);
                });

            migrationBuilder.CreateTable(
                name: "RoleTypes",
                columns: table => new
                {
                    RoleTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleTypes", x => x.RoleTypeId);
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
                    AppStatusId = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    LDAPName = table.Column<string>(nullable: true),
                    CreatorId = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: false),
                    LastModifiedById = table.Column<int>(nullable: true),
                    PositionId = table.Column<int>(nullable: false),
                    TempPositionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_Members_ApplicationStatuses_AppStatusId",
                        column: x => x.AppStatusId,
                        principalTable: "ApplicationStatuses",
                        principalColumn: "AppStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Members_Members_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Members_DutyStatuses_DutyStatusId",
                        column: x => x.DutyStatusId,
                        principalTable: "DutyStatuses",
                        principalColumn: "DutyStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "GenderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Members_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Members_Races_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Races",
                        principalColumn: "MemberRaceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Ranks_RankId",
                        column: x => x.RankId,
                        principalTable: "Ranks",
                        principalColumn: "RankId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    ComponentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentComponentId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Acronym = table.Column<string>(nullable: true),
                    LineupPosition = table.Column<int>(nullable: true),
                    CreatorId = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: false),
                    LastModifiedById = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_Components_Members_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Components_Members_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Components_Components_ParentComponentId",
                        column: x => x.ParentComponentId,
                        principalTable: "Components",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleTypeId = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_Roles_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Roles_RoleTypes_RoleTypeId",
                        column: x => x.RoleTypeId,
                        principalTable: "RoleTypes",
                        principalColumn: "RoleTypeId",
                        onDelete: ReferentialAction.Cascade);
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
                    IsAssistantManager = table.Column<bool>(nullable: false),
                    Callsign = table.Column<string>(nullable: true),
                    LineupPosition = table.Column<int>(nullable: true),
                    CreatorId = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: false),
                    LastModifiedById = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.PositionId);
                    table.ForeignKey(
                        name: "FK_Positions_Members_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Positions_Members_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Positions_Components_ParentComponentId",
                        column: x => x.ParentComponentId,
                        principalTable: "Components",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Components_CreatorId",
                table: "Components",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Components_LastModifiedById",
                table: "Components",
                column: "LastModifiedById");

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
                name: "IX_Members_AppStatusId",
                table: "Members",
                column: "AppStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_CreatorId",
                table: "Members",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_DutyStatusId",
                table: "Members",
                column: "DutyStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_GenderId",
                table: "Members",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_LastModifiedById",
                table: "Members",
                column: "LastModifiedById");

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
                name: "IX_Members_TempPositionId",
                table: "Members",
                column: "TempPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_CreatorId",
                table: "Positions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_LastModifiedById",
                table: "Positions",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_ParentComponentId",
                table: "Positions",
                column: "ParentComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_MemberId",
                table: "Roles",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleTypeId",
                table: "Roles",
                column: "RoleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Positions_PositionId",
                table: "Members",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "PositionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Positions_TempPositionId",
                table: "Members",
                column: "TempPositionId",
                principalTable: "Positions",
                principalColumn: "PositionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Members_CreatorId",
                table: "Components");

            migrationBuilder.DropForeignKey(
                name: "FK_Components_Members_LastModifiedById",
                table: "Components");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Members_CreatorId",
                table: "Positions");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Members_LastModifiedById",
                table: "Positions");

            migrationBuilder.DropTable(
                name: "ContactNumbers");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "PhoneNumberTypes");

            migrationBuilder.DropTable(
                name: "RoleTypes");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "ApplicationStatuses");

            migrationBuilder.DropTable(
                name: "DutyStatuses");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Races");

            migrationBuilder.DropTable(
                name: "Ranks");

            migrationBuilder.DropTable(
                name: "Components");
        }
    }
}
