using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;

namespace ForecastingSystem.Infrastructure.SyncDataAccess.Repositories
{
    public class SyncEmployeeRepository : SyncGenericAsyncRepository<Employee>, ISyncEmployeeRepository
    {
        public SyncEmployeeRepository(SyncForecastingSystemDbContext context) : base(context)
        {
        }
    }
}
