using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class OptimizePhaseResourceUtilisation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "ExternalProjectId",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "PhaseName",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "ResourcePlaceHolderId",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "ResourcePlaceHolderName",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "PhaseResourceUtilisation");

            migrationBuilder.RenameColumn(
                name: "FTE",
                table: "PhaseResourceUtilisation",
                newName: "TotalHours");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartWeek",
                table: "PhaseResourceUtilisation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_PhaseResourceUtilisation_PhaseResourceId",
                table: "PhaseResourceUtilisation",
                column: "PhaseResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhaseResourceUtilisation_PhaseResource_PhaseResourceId",
                table: "PhaseResourceUtilisation",
                column: "PhaseResourceId",
                principalTable: "PhaseResource",
                principalColumn: "PhaseResourceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhaseResourceUtilisation_PhaseResource_PhaseResourceId",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropIndex(
                name: "IX_PhaseResourceUtilisation_PhaseResourceId",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "StartWeek",
                table: "PhaseResourceUtilisation");

            migrationBuilder.RenameColumn(
                name: "TotalHours",
                table: "PhaseResourceUtilisation",
                newName: "FTE");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "PhaseResourceUtilisation",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "PhaseResourceUtilisation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExternalProjectId",
                table: "PhaseResourceUtilisation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhaseName",
                table: "PhaseResourceUtilisation",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "PhaseResourceUtilisation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "PhaseResourceUtilisation",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResourcePlaceHolderId",
                table: "PhaseResourceUtilisation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourcePlaceHolderName",
                table: "PhaseResourceUtilisation",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "PhaseResourceUtilisation",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);
        }
    }
}
