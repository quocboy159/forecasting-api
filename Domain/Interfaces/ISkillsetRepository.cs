using ForecastingSystem.Domain.Interfaces.Base;
using ForecastingSystem.Domain.Models;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface ISkillsetRepository : IBaseRepository<Skillset>
    {
        Task<Skillset> GetSkillsetByIdAsync(int skillsetId);

        Task<bool> IsSkillsetNameUniqueAsync(string skillsetName);
    }
}
