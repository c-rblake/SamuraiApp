using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class battledates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BattleSamurai_Battle_BattleId",
                table: "BattleSamurai");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Battle",
                table: "Battle");

            migrationBuilder.RenameTable(
                name: "Battle",
                newName: "Battles");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Battles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Battles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Battles",
                table: "Battles",
                column: "BattleId");

            migrationBuilder.AddForeignKey(
                name: "FK_BattleSamurai_Battles_BattleId",
                table: "BattleSamurai",
                column: "BattleId",
                principalTable: "Battles",
                principalColumn: "BattleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BattleSamurai_Battles_BattleId",
                table: "BattleSamurai");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Battles",
                table: "Battles");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Battles");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Battles");

            migrationBuilder.RenameTable(
                name: "Battles",
                newName: "Battle");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Battle",
                table: "Battle",
                column: "BattleId");

            migrationBuilder.AddForeignKey(
                name: "FK_BattleSamurai_Battle_BattleId",
                table: "BattleSamurai",
                column: "BattleId",
                principalTable: "Battle",
                principalColumn: "BattleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
