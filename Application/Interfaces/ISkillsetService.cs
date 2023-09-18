using ForecastingSystem.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface ISkillsetService
    {
        IList<SkillsetModel> GetSkillsets();
        bool IsExistingSkillset(int id);
        Task<SkillsetModel> AddAsync(SkillsetModel skillsetModel);

        Task<bool> IsSkillsetNameUniqueAsync(string skillsetName);
    }
}
