using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces
{
    public interface IPublicHolidayService
    {
        Task AddOrUpdateRangeAsync(IEnumerable<Outbound.PublicHoliday> tsProjectRateHistories, CancellationToken cancellationToken);
    }
}
