using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class NewTableForResourceUtilisation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "WeekStartDate",
                table: "PhaseResourceUtilisation");

            migrationBuilder.RenameColumn(
                name: "Hours",
                table: "PhaseResourceUtilisation",
                newName: "ResourcePlaceHolderId");

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
                name: "PhaseId",
                table: "PhaseResourceUtilisation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhaseName",
                table: "PhaseResourceUtilisation",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhaseResourceId",
                table: "PhaseResourceUtilisation",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddColumn<string>(
                name: "ResourcePlaceHolderName",
                table: "PhaseResourceUtilisation",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartWeek",
                table: "PhaseResourceUtilisation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "TotalHours",
                table: "PhaseResourceUtilisation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "PhaseResourceUtilisation",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ResourceUtilisationNote",
                columns: table => new
                {
                    ResourceUtilisationNoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    ResourcePlaceHolderId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    StartWeek = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceUtilisationNote", x => x.ResourceUtilisationNoteId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceUtilisationNote");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "PhaseId",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "PhaseName",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "PhaseResourceId",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "ResourcePlaceHolderName",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "StartWeek",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "TotalHours",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "PhaseResourceUtilisation");

            migrationBuilder.RenameColumn(
                name: "ResourcePlaceHolderId",
                table: "PhaseResourceUtilisation",
                newName: "Hours");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "PhaseResourceUtilisation",
                type: "varchar(max)",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WeekStartDate",
                table: "PhaseResourceUtilisation",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
