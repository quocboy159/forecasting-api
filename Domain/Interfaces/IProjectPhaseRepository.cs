using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IProjectPhaseRepository : IAsyncRepository<Phase>
    {
        Task<Phase> GetProjectPhaseByExternalPhaseId(int externalPhaseId);
        Phase GetPhaseToCalculateRevenueByTimeSheetPhaseId(int timesheetPhaseId);
        Task<bool> CheckExistResouceAssignmentsByExtenalPhaseId(int extenalPhaseId);
        Task<ICollection<Phase>> GetListByProjectIdAsync(int projectId);
        Task<Phase> GetProjectPhaseByIdAsync(int id);
        void DeleteSkillsets(IEnumerable<PhaseSkillset> phaseSkillsets);
        Task<Phase> GetPhaseToCalculateRevenue(int phaseId);

        Task<List<KeyValuePair<string, int>>> GetTimesheetPhaseIdListByProjectCodeAsync(string projectCode);
        Task<List<PhaseResource>> GetPhaseResourcesToCalculateUtilisation(int phaseId);

        Task<string> GetTimesheetPhaseCodeNameAsync(int timesheetPhaseId);
        Task UpdatePhaseValues(int phaseId , decimal phaseValue , decimal? budget , DateTime? estimatedEndDate);
        Task<int?> GetPhaseIdFromProjectPhaseId(int projectPhaseId);
        Task<Phase> GetLinkedOpportunityFromProjectPhaseId(int projectPhaseId);
    }
}
