using ForecastingSystem.Domain.Common;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class CleanDataOfEmployeeTimesheetEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE [EmployeeTimesheetEntry]");
            migrationBuilder.Sql($"DELETE [DataSyncProcess] WHERE DataSyncType = 'TimesheetSyncEntryJob'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
