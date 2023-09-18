using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface ISyncUserIdLookupRepository : IAsyncRepository<UserIdLookup>
    {
    }
}
