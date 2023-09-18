using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;

namespace ForecastingSystem.Infrastructure.SyncDataAccess.Repositories
{
    public class SyncProjectPhaseRepository : SyncGenericAsyncRepository<Phase>, ISyncProjectPhaseRepository
    {
        public SyncProjectPhaseRepository(SyncForecastingSystemDbContext context) : base(context)
        {
        }
    }
}
