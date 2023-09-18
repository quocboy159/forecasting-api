using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Models;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface IProjectPhaseService
    {
        Task<bool> CheckCanLinkToTimeSheetPhaseId(int phaseId,int timeSheetPhaseId);
        Task<ProjectPhaseModel> AddAsync(ProjectPhaseModel model);
        Task<bool> DeleteAsync(int id);
        Task<ProjectPhaseModel> EditAsync(ProjectPhaseModel model);
        Task<ProjectPhaseModel> GetProjectPhaseByIdAsync(int id);
        ProjectPhasesListModel GetProjectPhases(int projectId);
        Task<PhaseRevenueModel> GetPhaseRevenue(int phaseId);
        Task SavePhaseAndProjectValueAsync(ProjectPhaseModel model);
        Task SavePhaseRevenueValueAsync(PhaseRevenueModel phaseRevenue);
    }
}
