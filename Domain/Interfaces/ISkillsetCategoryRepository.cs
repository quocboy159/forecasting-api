using ForecastingSystem.Domain.Interfaces.Base;
using ForecastingSystem.Domain.Models;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface ISkillsetCategoryRepository : IBaseRepository<SkillsetCategory>
    {
        Task<int?> GetSkillsetCategoryIdByNameAsync(string skillsetCategoryName);
    }
    
}
