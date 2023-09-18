using AutoMapper;
using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.DataSyncServices.DataAccessServices
{
    public class ProjectRateService : IProjectRateService
    {
        private readonly ISyncProjectRepository _projectRepository;
        private readonly ISyncProjectRateRepository _projectRateRepository;
        private readonly IDataSyncServiceLogger _dataSyncLogger;
        private readonly IMapper _mapper;

        public ProjectRateService(ISyncProjectRepository projectRepository
            , ISyncProjectRateRepository projectRateRepository
            , IDataSyncServiceLogger dataSyncLogger
            , IMapper mapper)
        {
            _projectRepository = projectRepository;
            _projectRateRepository = projectRateRepository;
            _dataSyncLogger = dataSyncLogger;
            _mapper = mapper;
        }

        public async Task AddOrUpdateRangeAsync(IEnumerable<Outbound.ProjectRate> tsProjectRates, CancellationToken cancellationToken)
        {
            try 
            {
                var externalProjectRateIds = tsProjectRates.Select(x => x.ProjectId).ToList();
                var projects = await _projectRepository.GetAsync(x => externalProjectRateIds.Contains(x.ExternalProjectId ?? 0), cancellationToken);
                var missingProjectProjRates = new List<Outbound.ProjectRate>();
                tsProjectRates.ToList().ForEach(x => {
                    var project = projects.FirstOrDefault(y => y.ExternalProjectId == x.ProjectId);
                    if (project == null)
                    {
                        missingProjectProjRates.Add(x);
                        return;
                    }
                    x.FSProjectId = project.ProjectId;
                });
                tsProjectRates = tsProjectRates.Where(x => x.FSProjectId > 0);
                var allProjectRates = await _projectRateRepository.GetAllAsync(cancellationToken);

                var existingProjectRates = new List<ProjectRate>();
                var addedProjectRates = new List<ProjectRate>();
                tsProjectRates.ToList().ForEach(x => {
                    var existingItem = allProjectRates.FirstOrDefault(t => t.ExternalProjectRateId == x.RateId);
                    if (existingItem == null)
                    {
                        addedProjectRates.Add(_mapper.Map<ProjectRate>(x));
                    }
                    else
                    {
                        ProjectRate updatedItem = _mapper.Map<ProjectRate>(x);
                        updatedItem.ProjectRateId = existingItem.ProjectRateId;
                        existingProjectRates.Add(updatedItem);
                    }
                });

                await _projectRateRepository.UpdateRange(existingProjectRates);
                await _projectRateRepository.UpdateRange(addedProjectRates);
                await _projectRateRepository.SaveChangesAsync(cancellationToken);
                if (missingProjectProjRates.Any())
                    NotifyMissingProjects(missingProjectProjRates);
            }
            catch (CustomExceptions.CustomException ex)
            {
                _dataSyncLogger.LogWarning($"{GetType().Name} - SyncProcess skipped invalid data: {ex.Message}");
            }
        }

        private void NotifyMissingProjects(IEnumerable<Outbound.ProjectRate> missingProjectProjRates)
        {
            var missingItems = new Dictionary<int, string>();
            missingProjectProjRates.ToList().ForEach(
                x =>
                {
                    missingItems.Add(x.RateId, $"{x.ProjectId}/{x.ProjectName}");
                });
            var msg = String.Format("{0}: {1}", "ProjectRate without existing projects", missingItems.ToJsonString());
            throw new CustomExceptions.InvalidDataException(msg);
        }
    }
}
