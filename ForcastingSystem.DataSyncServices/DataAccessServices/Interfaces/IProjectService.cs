using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces
{
    public interface IProjectService
    {
        Task AddOrUpdateRangeAsync(IEnumerable<Outbound.Project> tsProjects, CancellationToken cancellationToken);
        Task<List<Project>> GetAll();
        Task<List<int>> GetSyncedProjectExternalIds();
    }
}
