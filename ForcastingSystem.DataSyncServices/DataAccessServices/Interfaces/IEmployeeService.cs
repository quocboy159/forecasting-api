using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces
{
    public interface IEmployeeService
    {
        Task AddOrUpdateRangeAsync(IEnumerable<Outbound.Employee> tsProjectRateHistories, CancellationToken cancellationToken);
    }
}
