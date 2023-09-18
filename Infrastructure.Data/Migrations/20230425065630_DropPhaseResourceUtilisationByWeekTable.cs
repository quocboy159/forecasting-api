using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class DropPhaseResourceUtilisationByWeekTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhaseResourceUtilisationByWeek");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
