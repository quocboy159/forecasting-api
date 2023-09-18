using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class HoursPerWeekResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "HoursPerWeek",
                table: "PhaseResource",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "HoursPerWeek",
                table: "PhaseResource",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
