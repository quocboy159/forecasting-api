using ForecastingSystem.Application.Models;
using System.Collections.Generic;

namespace ForecastingSystem.Application.Interfaces
{
    public interface IPhaseSkillsetService
    {
        IList<PhaseSkillsetModelToView> GetPhaseSkillsets(int phaseId);
        PhaseSkillsetModel Add(PhaseSkillsetModel model);
        PhaseSkillsetModel Update(PhaseSkillsetModel model);
        bool Delete(int id);
    }
}
