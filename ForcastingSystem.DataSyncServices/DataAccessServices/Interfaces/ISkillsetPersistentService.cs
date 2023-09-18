using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces
{
    public interface ISkillsetPersistentService
    {
        void Save(Skillset[] skillsets);
    }
}
