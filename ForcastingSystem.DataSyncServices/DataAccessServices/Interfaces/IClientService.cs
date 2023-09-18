using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces
{
    public interface IClientService
    {
        Task AddOrUpdateRangeAsync(IEnumerable<Outbound.Client> tsClients, CancellationToken cancellationToken);
    }
}
