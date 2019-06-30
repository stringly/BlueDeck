using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueDeck.Migrations
{
    public partial class FixMemVehNavProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Members_AssignedToMemberId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_AssignedToMemberId",
                table: "Vehicles");

            migrationBuilder.CreateIndex(
                name: "IX_Members_AssignedVehicleId",
                table: "Members",
                column: "AssignedVehicleId",
                unique: true,
                filter: "[AssignedVehicleId] IS NOT NULL");

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

            migrationBuilder.DropIndex(
                name: "IX_Members_AssignedVehicleId",
                table: "Members");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_AssignedToMemberId",
                table: "Vehicles",
                column: "AssignedToMemberId",
                unique: true,
                filter: "[AssignedToMemberId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Members_AssignedToMemberId",
                table: "Vehicles",
                column: "AssignedToMemberId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
