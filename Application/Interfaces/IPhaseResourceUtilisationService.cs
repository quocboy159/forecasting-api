using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface IPhaseResourceUtilisationService
    {
        Task GeneratePhaseResourceUtilisations(int phaseId);
        Task<IEnumerable<PhaseResourceUtilisation>> DeletePhaseResourceUtilisations(int phaseId);
    }
}
