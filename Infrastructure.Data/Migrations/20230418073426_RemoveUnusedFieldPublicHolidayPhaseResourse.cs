using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class RemoveUnusedFieldPublicHolidayPhaseResourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhaseResource_PhaseResourceUtilisation",
                table: "PhaseResource");

            migrationBuilder.DropIndex(
                name: "IX_PhaseResource_UtilisationId",
                table: "PhaseResource");

            migrationBuilder.DropColumn(
                name: "LastSyncDate",
                table: "PublicHoliday");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "PhaseResource");

            migrationBuilder.DropColumn(
                name: "LastModiffiedBy",
                table: "PhaseResource");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "PhaseResource");

            migrationBuilder.DropColumn(
                name: "LastSyncDate",
                table: "PhaseResource");

            migrationBuilder.DropColumn(
                name: "ResourceStatus",
                table: "PhaseResource");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "PhaseResource");

            migrationBuilder.DropColumn(
                name: "UtilisationId",
                table: "PhaseResource");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastSyncDate",
                table: "PublicHoliday",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "PhaseResource",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModiffiedBy",
                table: "PhaseResource",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "PhaseResource",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSyncDate",
                table: "PhaseResource",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourceStatus",
                table: "PhaseResource",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "PhaseResource",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UtilisationId",
                table: "PhaseResource",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhaseResource_UtilisationId",
                table: "PhaseResource",
                column: "UtilisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhaseResource_PhaseResourceUtilisation",
                table: "PhaseResource",
                column: "UtilisationId",
                principalTable: "PhaseResourceUtilisation",
                principalColumn: "PhaseResourceUtilisationId");
        }
    }
}
