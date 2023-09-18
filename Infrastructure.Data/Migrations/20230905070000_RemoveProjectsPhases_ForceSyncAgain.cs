using ForecastingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;
using static ForecastingSystem.Domain.Common.Constants;

#nullable disable

namespace ForecastingSystem.Infrastructure.Data.Migrations
{
    public partial class RemoveProjectsPhases_ForceSyncAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DECLARE @to_delete_projects_table TABLE ( ProjectId INT NOT NULL                );
                DECLARE @to_delete_phases_table TABLE ( PhaseId INT NOT NULL );

                INSERT INTO @to_delete_projects_table
                            SELECT[ProjectId]
                FROM[Project]
                  WHERE ExternalProjectId NOT IN (166,171,224,263,328,333,405,458,461,465,472,484,485,487,491,492,501,512,517,519,520,524,529,531,534,536,558,559,560,567,571,573,574,575,576,577,578,579,580,589,591,592,594,595,597,600,601,602,605,606,608,610,612,613,614,620,621,625,627,630,631,632,634,635,636,638,639,642,646,649,650,653,655,656,657,658,659,1661,1662,1663,1667,1668,1669,1671,1673,1674,1675,1676,1677,1678,1679,1680,1684,1685,1686,1687,1688,1689,1690,1691,1692,1693,1694,1695,1696,1697,1698,1699,1700,1701,1703,1704,1705,1706,1707,1708,1709,1710,1711,1712)

                INSERT INTO @to_delete_phases_table
                SELECT PhaseId
                  FROM Phase INNER JOIN @to_delete_projects_table Pj ON Pj.ProjectId = Phase.ProjectId



                Delete [PhaseSkillset] WHERE PhaseId IN(Select PhaseId from @to_delete_phases_table)
                Delete PhaseResourceUtilisation WHERE PhaseId IN(Select PhaseId from @to_delete_phases_table)
                Delete [PhaseResourceException] WHERE[PhaseResourceId] IN(Select PhaseResource.PhaseResourceID from PhaseResource INNER JOIN @to_delete_phases_table P ON PhaseResource.PhaseId = P.PhaseId
                                                                              )
                Delete [PhaseResource] WHERE PhaseId IN(Select PhaseId from @to_delete_phases_table)
                Delete [Phase] WHERE PhaseId IN(Select PhaseId from @to_delete_phases_table)

                IF OBJECT_ID (N'ProjectEmployeeManager', N'U') IS NOT NULL    
                 Delete [ProjectEmployeeManager] WHERE[ProjectId]  IN(Select[ProjectId] from @to_delete_projects_table)

                DELETE ProjectRateHistory
                FROM ProjectRate INNER JOIN
                         ProjectRateHistory ON ProjectRate.ProjectRateId = ProjectRateHistory.ProjectRateId INNER JOIN
                         @to_delete_projects_table Project ON ProjectRate.ProjectId = Project.ProjectId

                DELETE ProjectRate
                FROM ProjectRate INNER JOIN
                         @to_delete_projects_table Project ON ProjectRate.ProjectId = Project.ProjectId

                DELETE Project
                FROM Project INNER JOIN
                         @to_delete_projects_table P ON P.ProjectId = Project.ProjectId
                
                DELETE [EmployeeTimesheetEntry]
                ");

            migrationBuilder.Sql($"UPDATE [DataSyncProcess] SET LastSyncDateTime = '1800-01-01' WHERE DataSyncType = '{DataSyncTypes.TimesheetSyncProjectJob}'");
            migrationBuilder.Sql($"UPDATE [DataSyncProcess] SET LastSyncDateTime = '1800-01-01' WHERE DataSyncType = '{DataSyncTypes.TimesheetSyncPhaseJob}'");
            migrationBuilder.Sql($"UPDATE [DataSyncProcess] SET LastSyncDateTime = '1800-01-01' WHERE DataSyncType = '{DataSyncTypes.TimesheetSyncProjectRateHistoryJob}'");
            migrationBuilder.Sql($"UPDATE [DataSyncProcess] SET LastSyncDateTime = '1800-01-01' WHERE DataSyncType = '{DataSyncTypes.TimesheetSyncProjectRateJob}'");
            migrationBuilder.Sql($"UPDATE [DataSyncProcess] SET LastSyncDateTime = '1800-01-01' WHERE DataSyncType = '{DataSyncTypes.TimesheetSyncEntryJob}'");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
