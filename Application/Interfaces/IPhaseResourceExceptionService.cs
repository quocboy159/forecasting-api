using ForecastingSystem.Application.Models;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface IPhaseResourceExceptionService
    {
        Task<PhaseResourceExceptionModelToView> AddAsync(PhaseResourceExceptionModel model);
        Task<bool> DeleteAsync(int id);
        Task<PhaseResourceExceptionModelToView> EditAsync(PhaseResourceExceptionModel model);
        Task<PhaseResourceExceptionModelToView> GetPhaseResourceExceptionByIdAsync(int phaseResouceExceptionId);
        Task<PhaseResourceExceptionListModel> GetPhaseResourceExceptionsByPhaseIdAsync(int phaseId);
    }
}
