using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces
{
    public interface IEmployeeLeaveService
    {
        Task AddOrUpdateRangeAsync(IEnumerable<Outbound.EmployeeLeave> tsProjectRateHistories, CancellationToken cancellationToken);
    }
}
