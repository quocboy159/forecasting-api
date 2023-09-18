using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class UpdatePhaseResourceUtilisation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartWeek",
                table: "PhaseResourceUtilisation");

            migrationBuilder.DropColumn(
                name: "TotalHours",
                table: "PhaseResourceUtilisation");

            migrationBuilder.AddColumn<int>(
                name: "ExternalProjectId",
                table: "PhaseResourceUtilisation",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PhaseResourceUtilisationByWeek",
                columns: table => new
                {
                    PhaseResourceUtilisationByWeekId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhaseResourceUtilisationId = table.Column<int>(type: "int", nullable: false),
                    StartWeek = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalHours = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhaseResourceUtilisationByWeek", x => x.PhaseResourceUtilisationByWeekId);
                    table.ForeignKey(
                        name: "FK_PhaseResourceUtilisationByWeek_PhaseResourceUtilisation_PhaseResourceUtilisationId",
                        column: x => x.PhaseResourceUtilisationId,
                        principalTable: "PhaseResourceUtilisation",
                        principalColumn: "PhaseResourceUtilisationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhaseResourceUtilisationByWeek_PhaseResourceUtilisationId",
                table: "PhaseResourceUtilisationByWeek",
                column: "PhaseResourceUtilisationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhaseResourceUtilisationByWeek");

            migrationBuilder.DropColumn(
                name: "ExternalProjectId",
                table: "PhaseResourceUtilisation");

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
        }
    }
}
