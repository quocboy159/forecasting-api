using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class DataSyncProcessRepository : SyncGenericAsyncRepository<DataSyncProcess>, IDataSyncProcessRepository
    {
        public DataSyncProcessRepository(SyncForecastingSystemDbContext context) : base(context)
        {
        }
    }
}
