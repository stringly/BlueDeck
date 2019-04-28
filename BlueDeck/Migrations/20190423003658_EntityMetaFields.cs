using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlueDeck.Migrations
{
    public partial class EntityMetaFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Positions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Positions",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Positions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                table: "Positions",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Members",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Members",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Components",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Components",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Components",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedById",
                table: "Components",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_CreatorId",
                table: "Positions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_LastModifiedById",
                table: "Positions",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Members_CreatorId",
                table: "Members",
                column: "CreatorId",
                unique: true,
                filter: "[CreatorId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Members_LastModifiedById",
                table: "Members",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Components_CreatorId",
                table: "Components",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Components_LastModifiedById",
                table: "Components",
                column: "LastModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Members_CreatorId",
                table: "Components",
                column: "CreatorId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Members_LastModifiedById",
                table: "Components",
                column: "LastModifiedById",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Members_CreatorId",
                table: "Members",
                column: "CreatorId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Members_LastModifiedById",
                table: "Members",
                column: "LastModifiedById",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Members_CreatorId",
                table: "Positions",
                column: "CreatorId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Members_LastModifiedById",
                table: "Positions",
                column: "LastModifiedById",
                principalTable: "Members",
                principalColumn: "MemberId",
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
                name: "FK_Members_Members_CreatorId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Members_LastModifiedById",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Members_CreatorId",
                table: "Positions");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Members_LastModifiedById",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Positions_CreatorId",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Positions_LastModifiedById",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Members_CreatorId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_LastModifiedById",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Components_CreatorId",
                table: "Components");

            migrationBuilder.DropIndex(
                name: "IX_Components_LastModifiedById",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Components");
        }
    }
}
