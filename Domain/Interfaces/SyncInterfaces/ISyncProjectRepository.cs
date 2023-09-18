using ForecastingSystem.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface ISyncProjectRepository : IAsyncRepository<Project>
    {
        Task ClearProjectValue(List<int> projectIds);
    }
}
