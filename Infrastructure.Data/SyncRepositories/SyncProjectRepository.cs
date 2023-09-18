using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.SyncDataAccess.Repositories
{
    public class SyncProjectRepository : SyncGenericAsyncRepository<Project>, ISyncProjectRepository
    {
        public SyncProjectRepository(SyncForecastingSystemDbContext context) : base(context)
        {
        }

        public async Task ClearProjectValue(List<int> projectIds)
        {
            string joinedProjectIds = string.Join("," , projectIds);
                string sql = "UPDATE Project " +
                                 "SET " +
                        "             IsObsoleteProjectValue = 0  , ProjectValue = 0  " +
                        $"        WHERE ProjectId IN ({joinedProjectIds})";

                var projectIdParam = new SqlParameter("@joinedProjectIds" , joinedProjectIds);
                int rowsAffected = await DbContext.Database.ExecuteSqlRawAsync(sql , projectIdParam);
        }
    }
}
