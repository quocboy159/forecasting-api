using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class TimesheetUsernameEmployeeLeave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimsheetUsername",
                table: "EmployeeLeave",
                newName: "TimesheetUsername");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimesheetUsername",
                table: "EmployeeLeave",
                newName: "TimsheetUsername");
        }
    }
}
