using Microsoft.EntityFrameworkCore.Migrations;

namespace OrgChartDemo.Migrations
{
    public partial class ParentComponentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Components_ParentComponentComponentId",
                table: "Components");

            migrationBuilder.RenameColumn(
                name: "ParentComponentComponentId",
                table: "Components",
                newName: "ParentComponentId");

            migrationBuilder.RenameIndex(
                name: "IX_Components_ParentComponentComponentId",
                table: "Components",
                newName: "IX_Components_ParentComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Components_ParentComponentId",
                table: "Components",
                column: "ParentComponentId",
                principalTable: "Components",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Components_ParentComponentId",
                table: "Components");

            migrationBuilder.RenameColumn(
                name: "ParentComponentId",
                table: "Components",
                newName: "ParentComponentComponentId");

            migrationBuilder.RenameIndex(
                name: "IX_Components_ParentComponentId",
                table: "Components",
                newName: "IX_Components_ParentComponentComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Components_ParentComponentComponentId",
                table: "Components",
                column: "ParentComponentComponentId",
                principalTable: "Components",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
