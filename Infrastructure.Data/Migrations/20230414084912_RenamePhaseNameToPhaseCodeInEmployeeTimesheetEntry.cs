using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class RenamePhaseNameToPhaseCodeInEmployeeTimesheetEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhaseName",
                table: "EmployeeTimesheetEntry",
                newName: "PhaseCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhaseCode",
                table: "EmployeeTimesheetEntry",
                newName: "PhaseName");
        }
    }
}
