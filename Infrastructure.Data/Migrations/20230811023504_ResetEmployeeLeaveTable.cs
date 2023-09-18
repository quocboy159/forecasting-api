using Microsoft.EntityFrameworkCore.Migrations;
using static ForecastingSystem.Domain.Common.Constants;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class ResetEmployeeLeaveTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"TRUNCATE TABLE [dbo].[EmployeeLeave]
                                    GO
                                    UPDATE [dbo].[DataSyncProcess] SET LastSyncDateTime = '1800-01-01' WHERE DataSyncType = '{DataSyncTypes.TimesheetSyncEmployeeLeaveJob}'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
