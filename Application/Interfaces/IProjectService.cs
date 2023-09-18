using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface IProjectService
    {
        Task<bool> CheckProjectHasLinkedPhase(int projectId);
        Task<Project> SaveAsync(ProjectDetailAddEditModel model);
        Task<ProjectListModel> GetAllProjectsAsync();
        Task<ProjectModel> GetProjectByIdAsync(int projectId);
        bool IsOpportunity(int projectId);
        Task<ProjectDetailModel> GetProjectDetailAsync(int projectId);
        Task<ProjectRevenueModel> GetProjectRevenue(int projectId);
        Task<List<ProjectRevenueModel>> GetProjectRevenues(List<int> projectIds);
        Task<(List<ProjectRevenueModel>, List<string> consumedTimes)> GetProjectsRevenueForecast(DateTime startWeek, int maxNumberOfWeeks = 52, int maxNumberOfMonths = 12);
        Task<List<string>> GetProjectCodeListAsync();
        Task<bool> IsUserHasAccessToProjectAsync(int id);
    }
}
