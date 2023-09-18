using ForecastingSystem.Application.Models;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface IPhaseResourceService
    {
        Task<PhaseResourceModelToView> AddAsync(PhaseResourceModel model);
        Task<bool> DeleteAsync(int id);
        Task<PhaseResourceModel> EditAsync(PhaseResourceModel model);
        Task<PhaseResourceModelToView> GetPhaseResourceByIdAsync(int id);
        Task<PhaseResourceListModel> GetPhaseResourcesByPhaseIdAsync(int phaseId);
        Task<PhaseResourceListModel> GetEmployeePhaseResourcesByPhaseIdAsync(int phaseId);
        Task<bool> IsPhaseResourceUsedByProjectRateIdAsync(int projectRateId);
        Task<PhaseResourceListModel> GetEmployeePhaseResourcesByProjectPhaseIdAsync(int projectPhaseId);
    }
}
