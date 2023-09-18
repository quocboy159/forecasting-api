using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;

namespace ForecastingSystem.Infrastructure.SyncDataAccess.Repositories
{
    public class SyncProjectRateRepository : SyncGenericAsyncRepository<ProjectRate>, ISyncProjectRateRepository
    {
        public SyncProjectRateRepository(SyncForecastingSystemDbContext context) : base(context)
        {
        }
    }
}
