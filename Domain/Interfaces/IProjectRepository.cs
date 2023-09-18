using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IProjectRepository : IAsyncRepository<Project>
    {
        Task<Project> GetProjectDetailAsync(int id);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<List<Project>> GetProjectsToCalculateRevenue(List<int> projectIds = null,
                                                            DateTime? startDate = null,
                                                            DateTime? endDate = null,
                                                            ProjectStatus? status = null,
                                                            bool? excludeLinkedOpportunityToProject = null,
                                                            bool? isClosed = null,
                                                            bool? isCompleted = null);
        Task<List<string>> GetProjectCodeListAsync();
        Task<IEnumerable<Project>> GetProjectsByUserAsync(string username);
        Task<bool> IsUserHasAccessToProjectAsync(int id, string username, string role);
        Task<bool> IsOpportunityAsync(int projectId);
        Task ClearProjectValue(int projectId);
        Task UpdateProjectValues(int projectId , decimal projectValue , decimal projectBudget);
    }
}
