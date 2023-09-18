using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class FixPublicHolidayId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublidHolidayId",
                table: "PublicHoliday",
                newName: "PublicHolidayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublicHolidayId",
                table: "PublicHoliday",
                newName: "PublidHolidayId");
        }
    }
}
