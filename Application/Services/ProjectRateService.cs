using AutoMapper;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Services
{
    public class ProjectRateService : IProjectRateService
    {
        private readonly IProjectRateRepository _projectRateRepository;
        private readonly IProjectRateHistoryRepository _projectRateHistoryRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        public ProjectRateService(IProjectRateRepository projectRateRepository,
                                  IMapper mapper,
                                  IProjectRateHistoryRepository projectRateHistoryRepository,
                                  IProjectRepository projectRepository)
        {
            _mapper = mapper;
            _projectRateRepository = projectRateRepository;
            _projectRateHistoryRepository= projectRateHistoryRepository;
            _projectRepository = projectRepository;
        }

        public async Task<ProjectRatesListModel> GetProjectRatesAsync(int projectId)
        {
            var rates = await _projectRateRepository.GetAsync(x => x.ProjectId == projectId);

            var projectRates = _mapper.Map<IEnumerable<ProjectRateModel>>(rates);
            foreach (var rate in projectRates)
            {
                ProjectRateHistory currentRate = await GetCurrentRateHistoryAsync(rate.ProjectRateId);
                rate.EffectiveDate = currentRate?.StartDate;
                rate.Rate = (decimal?)(currentRate?.Rate?? 0);
            }

            return new ProjectRatesListModel()
            {
                ProjectRates = projectRates
            };
        }

        public async Task<ProjectRateHistory> GetCurrentRateHistoryAsync(int rateId)
        {
            var projectRateHistory =  await _projectRateRepository.GetCurrentRateAsync(rateId);

            return projectRateHistory; 
        }

        public async Task<ProjectRateHistory> GetMostRecentRateValueForSepecificDateAsync(int rateId, DateTime startDate)
        {
            var rate = await _projectRateRepository.GetMostRecentRateValueForSepecificDateAsync(rateId, startDate);
            return rate;
        }

        public async Task<ProjectRatesListModel> GetProjectRatesByProjectIdAsync(int projectId)
        {
            var rates = await _projectRateRepository.GetAsync(x => x.ProjectId == projectId && x.Status == ProjectRateStatus.Active);
            var projectRates = _mapper.Map<IEnumerable<ProjectRateModel>>(rates);
            return new ProjectRatesListModel()
            {
                ProjectRates = projectRates
            };
        }

        public async Task<List<ProjectRateHistoryGroupModel>> GetRateHistoriesByProjectIdAsync(int projectId)
        {
            var project = await _projectRateRepository.GetRateHistoriesByProjectIdAsync(projectId);

            var projectRateHistoryGroupModel = _mapper.Map<List<ProjectRateHistoryGroupModel>>(project);

            return projectRateHistoryGroupModel;
        }

        public async Task<List<int>> GetRateIdsByProjectIdAsync(int projectId)
        {
            var result = await _projectRateRepository.GetRateIdsByProjectIdAsync(projectId);

            return result;
        }

        public async Task<string> GetRateNameById(int projectRateId)
        {
            var result = await _projectRateRepository.GetRateNameById(projectRateId);

            return result;
        }

        public async Task<bool> IsActiveProjectRateAsync(int projectRateId)
        {
            var result = await _projectRateRepository.IsActiveProjectRateAsync(projectRateId);

            return result;
        }
    }
}
