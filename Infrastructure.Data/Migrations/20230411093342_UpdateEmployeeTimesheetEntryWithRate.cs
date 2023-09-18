using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class UpdateEmployeeTimesheetEntryWithRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExternalRateId",
                table: "EmployeeTimesheetEntry",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "RateAmount",
                table: "EmployeeTimesheetEntry",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RateName",
                table: "EmployeeTimesheetEntry",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalRateId",
                table: "EmployeeTimesheetEntry");

            migrationBuilder.DropColumn(
                name: "RateAmount",
                table: "EmployeeTimesheetEntry");

            migrationBuilder.DropColumn(
                name: "RateName",
                table: "EmployeeTimesheetEntry");
        }
    }
}
