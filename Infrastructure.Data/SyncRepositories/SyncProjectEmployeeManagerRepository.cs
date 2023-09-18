using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;

namespace ForecastingSystem.Infrastructure.SyncDataAccess.Repositories
{
    public class SyncProjectEmployeeManagerRepository : SyncGenericAsyncRepository<ProjectEmployeeManager>, ISyncProjectEmployeeManagerRepository
    {
        public SyncProjectEmployeeManagerRepository(SyncForecastingSystemDbContext context) : base(context)
        {
        }
    }
}
