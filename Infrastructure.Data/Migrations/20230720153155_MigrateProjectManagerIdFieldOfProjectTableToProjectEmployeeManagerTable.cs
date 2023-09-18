using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class MigrateProjectManagerIdFieldOfProjectTableToProjectEmployeeManagerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO [dbo].[ProjectEmployeeManager]([ProjectId], [EmployeeId])
                                    SELECT ProjectId, ProjectManagerId
                                    FROM [dbo].[Project] 
                                    WHERE ProjectType = 'Opportunity'
                                    AND ProjectManagerId IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
