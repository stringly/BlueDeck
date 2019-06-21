using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueDeck.Migrations
{
    public partial class vehicle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedVehicleId",
                table: "Members",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VehicleManufacturers",
                columns: table => new
                {
                    VehicleManufacturerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VehicleManufacturerName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleManufacturers", x => x.VehicleManufacturerId);
                });

            migrationBuilder.CreateTable(
                name: "VehicleModels",
                columns: table => new
                {
                    VehicleModelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VehicleModelName = table.Column<string>(nullable: true),
                    ManufacturerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleModels", x => x.VehicleModelId);
                    table.ForeignKey(
                        name: "FK_VehicleModels_VehicleManufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "VehicleManufacturers",
                        principalColumn: "VehicleManufacturerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModelYear = table.Column<int>(nullable: false),
                    ModelId = table.Column<int>(nullable: false),
                    VIN = table.Column<string>(nullable: true),
                    TagNumber = table.Column<string>(nullable: true),
                    TagState = table.Column<string>(nullable: true),
                    CruiserNumber = table.Column<string>(nullable: true),
                    AssignedToMemberId = table.Column<int>(nullable: true),
                    AssignedToPositionId = table.Column<int>(nullable: true),
                    AssignedToComponentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_Vehicles_Components_AssignedToComponentId",
                        column: x => x.AssignedToComponentId,
                        principalTable: "Components",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_Positions_AssignedToPositionId",
                        column: x => x.AssignedToPositionId,
                        principalTable: "Positions",
                        principalColumn: "PositionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleModels_ModelId",
                        column: x => x.ModelId,
                        principalTable: "VehicleModels",
                        principalColumn: "VehicleModelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_AssignedVehicleId",
                table: "Members",
                column: "AssignedVehicleId",
                unique: true,
                filter: "[AssignedVehicleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleModels_ManufacturerId",
                table: "VehicleModels",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_AssignedToComponentId",
                table: "Vehicles",
                column: "AssignedToComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_AssignedToPositionId",
                table: "Vehicles",
                column: "AssignedToPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ModelId",
                table: "Vehicles",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Vehicles_AssignedVehicleId",
                table: "Members",
                column: "AssignedVehicleId",
                principalTable: "Vehicles",
                principalColumn: "VehicleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Vehicles_AssignedVehicleId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "VehicleModels");

            migrationBuilder.DropTable(
                name: "VehicleManufacturers");

            migrationBuilder.DropIndex(
                name: "IX_Members_AssignedVehicleId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "AssignedVehicleId",
                table: "Members");
        }
    }
}
