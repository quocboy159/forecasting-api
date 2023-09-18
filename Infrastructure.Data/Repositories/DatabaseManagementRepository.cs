using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Infrastructure.Data.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class DatabaseManagementRepository : IDatabaseManagementRepository
    {
        private ForecastingSystemDbContext _dbContext;

        public DatabaseManagementRepository(ForecastingSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DropTables()
        {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var sql = System.IO.File.ReadAllText(path + "/_SQLScripts/DropAllTables.sql");
            await _dbContext.Database.ExecuteSqlRawAsync(sql);
        }

        public async Task RecreateTables()
        {
            await _dbContext.Database.MigrateAsync();
        }

        public async Task ResetDataSyncTypeAsync(string dataSyncType)
        {
            string sql = "UPDATE [DataSyncProcess] SET LastSyncDateTime = '1800-01-01' WHERE DataSyncType = {0}";
            await _dbContext.Database.ExecuteSqlRawAsync(sql, dataSyncType);
        }

    }
}
