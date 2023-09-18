using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class AddExternalLeaveHolidayId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExternalLeaveHolidayId",
                table: "PublicHoliday",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalLeaveHolidayId",
                table: "PublicHoliday");
        }
    }
}
