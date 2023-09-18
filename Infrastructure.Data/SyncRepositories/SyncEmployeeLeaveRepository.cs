using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;

namespace ForecastingSystem.Infrastructure.SyncDataAccess.Repositories
{
    public class SyncEmployeeLeaveRepository : SyncGenericAsyncRepository<EmployeeLeave>, ISyncEmployeeLeaveRepository
    {
        public SyncEmployeeLeaveRepository(SyncForecastingSystemDbContext context) : base(context)
        {
        }
    }
}
