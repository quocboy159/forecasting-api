using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class ProjectPhaseValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsObsoleteProjectValue",
                table: "Project",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProjectValue" ,
                table: "Project" ,
                type: "decimal(18,2)" ,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstimatedEndDate" ,
                table: "Phase" ,
                type: "datetime2" ,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PhaseValue" ,
                table: "Phase" ,
                type: "decimal(18,2)" ,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsObsoleteProjectValue",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectValue",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "EstimatedEndDate",
                table: "Phase");

            migrationBuilder.DropColumn(
                name: "PhaseValue",
                table: "Phase");
        }
    }
}
