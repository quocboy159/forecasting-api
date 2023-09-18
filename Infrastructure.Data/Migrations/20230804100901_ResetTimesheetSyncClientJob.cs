﻿using Microsoft.EntityFrameworkCore.Migrations;
using static ForecastingSystem.Domain.Common.Constants;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class ResetTimesheetSyncClientJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"UPDATE [DataSyncProcess] SET LastSyncDateTime = '1800-01-01' WHERE DataSyncType = '{DataSyncTypes.TimesheetSyncClientJob}'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
