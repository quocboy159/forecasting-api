using ForecastingSystem.Domain.Interfaces.Base;
using ForecastingSystem.Domain.Models;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface ISyncSkillsetCategoryRepository : IAsyncRepository<SkillsetCategory>
    {
        int CreateSkillsetCategoryIfNotExist(string skillsetCategoryName);
    }
}
