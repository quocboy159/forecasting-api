using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface IProjectRateService
    {
        Task<ProjectRatesListModel> GetProjectRatesAsync(int projectId);
        Task<ProjectRateHistory> GetCurrentRateHistoryAsync(int rateId);
        Task<ProjectRatesListModel> GetProjectRatesByProjectIdAsync(int projectId);
        Task<List<ProjectRateHistoryGroupModel>> GetRateHistoriesByProjectIdAsync(int projectId);
        Task<List<int>> GetRateIdsByProjectIdAsync(int projectId);
        Task<string> GetRateNameById(int projectRateId);
        Task<bool> IsActiveProjectRateAsync(int projectRateId);
        Task<ProjectRateHistory> GetMostRecentRateValueForSepecificDateAsync(int rateId, DateTime startDate);
    }
}
