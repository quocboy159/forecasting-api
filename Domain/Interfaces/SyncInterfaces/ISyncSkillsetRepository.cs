using ForecastingSystem.Domain.Interfaces.Base;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface ISyncSkillsetRepository : IAsyncRepository<Skillset>
    {
    }
}
