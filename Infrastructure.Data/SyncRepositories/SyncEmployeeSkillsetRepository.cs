using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;

namespace ForecastingSystem.Infrastructure.SyncDataAccess.Repositories
{
    public class SyncEmployeeSkillsetRepository : SyncGenericAsyncRepository<EmployeeSkillset>, ISyncEmployeeSkillsetRepository
    {
        public SyncEmployeeSkillsetRepository(SyncForecastingSystemDbContext dbContext) : base(dbContext)
        {
            /*
                This is the place where we create the logic for query,
                for saving and calling data for that entity.
             */
        }
    }
}
