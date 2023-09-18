using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class Add_NumberOfWeeks_Remove_EndWeek_In_PhaseResourceException : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndWeek",
                table: "PhaseResourceException");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWeeks",
                table: "PhaseResourceException",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfWeeks",
                table: "PhaseResourceException");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndWeek",
                table: "PhaseResourceException",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
