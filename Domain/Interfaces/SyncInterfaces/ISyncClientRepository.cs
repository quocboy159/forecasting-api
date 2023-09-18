using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface ISyncClientRepository : IAsyncRepository<Client>
    {}
}
