using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IProjectRateRepository : IAsyncRepository<ProjectRate>
    {
        // This is where we put the methods specific for that class
        Task<IList<ProjectRate>> GetProjectRatesFromResouceAssignmentsByPhaseId(int phaseId);
        Task<IList<ProjectRate>> GetProjectRatesByExternalPhaseId(int externalPhaseId);
        Task<IList<ProjectRateHistory>> GetCurrentRatesAsync(IEnumerable<int> rateIds);
        Task<ProjectRateHistory> GetCurrentRateAsync(int rateId);
        Task<List<ProjectRate>> GetRateHistoriesByProjectIdAsync(int projectId);
        Task<List<int>> GetRateIdsByProjectIdAsync(int projectId);
        Task<string> GetRateNameById(int projectRateId);
        Task<bool> IsActiveProjectRateAsync(int projectRateId);
        Task<ProjectRateHistory> GetMostRecentRateValueForSepecificDateAsync(int rateId, DateTime startDate);
        Task<IList<ProjectRate>> GetOpportunityActiveProjectRatesByProjectCode(string projectCode);
    }
}
