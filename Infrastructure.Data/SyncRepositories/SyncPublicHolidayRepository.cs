using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;

namespace ForecastingSystem.Infrastructure.SyncDataAccess.Repositories
{
    public class SyncPublicHolidayRepository : SyncGenericAsyncRepository<PublicHoliday>, ISyncPublicHolidayRepository
    {
        public SyncPublicHolidayRepository(SyncForecastingSystemDbContext context) : base(context)
        {
        }
    }
}
