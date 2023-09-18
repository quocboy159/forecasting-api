using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IProjectRateHistoryRepository : IAsyncRepository<ProjectRateHistory>
    {
        // This is where we put the methods specific for that class
    }
}
