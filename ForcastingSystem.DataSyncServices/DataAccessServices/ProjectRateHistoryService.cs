using AutoMapper;
using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using System.Threading;

namespace ForecastingSystem.DataSyncServices.DataAccessServices
{
    public class ProjectRateHistoryService : IProjectRateHistoryService
    {
        private readonly ISyncProjectRateRepository _projectRateRepository;
        private readonly ISyncProjectRateHistoryRepository _projectRateHistoryRepository;
        private readonly ISyncProjectRepository _projectRepository;
        private readonly IDataSyncServiceLogger _dataSyncLogger;
        private readonly IMapper _mapper;

        public ProjectRateHistoryService(
            ISyncProjectRateHistoryRepository projectRateHistoryRepository
            , ISyncProjectRateRepository projectRateRepository
            , ISyncProjectRepository projectRepository
            , IDataSyncServiceLogger dataSyncLogger
            , IMapper mapper)
        {
            _projectRateHistoryRepository = projectRateHistoryRepository;
            _projectRateRepository = projectRateRepository;
            _projectRepository = projectRepository;
            _dataSyncLogger = dataSyncLogger;
            _mapper = mapper;
        }
        public async Task AddOrUpdateRangeAsync(IEnumerable<Outbound.ProjectRateHistory> tsProjectRateHistories, CancellationToken cancellationToken)
        {
            try 
            {
                var externalProjectRateIds = tsProjectRateHistories.Select(x => x.RateId).ToList();
                var projectRates = await _projectRateRepository.GetAsync(x => externalProjectRateIds.Contains(x.ExternalProjectRateId ?? 0), cancellationToken);
                var missingProjectRatePRHistories = new List<Outbound.ProjectRateHistory>();
                tsProjectRateHistories.ToList().ForEach(x => {
                    var projectRate = projectRates.FirstOrDefault(y => y.ExternalProjectRateId == x.RateId);
                    if (projectRate == null)
                    {
                        missingProjectRatePRHistories.Add(x);
                        return;
                    }
                    x.FSProjectRateId = projectRate.ProjectRateId;
                });
                tsProjectRateHistories = tsProjectRateHistories.Where(x => x.FSProjectRateId > 0);
                var allProjectRateHistories = await _projectRateHistoryRepository.GetAllAsync(cancellationToken);

                var existingProjectRateHistories = new List<ProjectRateHistory>();
                var addedProjectRateHistories = new List<ProjectRateHistory>();
                var rateChangedProjectRateIds = new List<int>();
                tsProjectRateHistories.ToList().ForEach(x => {
                    var existingItem = allProjectRateHistories.FirstOrDefault(t => t.ExternalRateHistoryId == x.RateHistoryId);
                    if (existingItem == null)
                    {
                        addedProjectRateHistories.Add(_mapper.Map<ProjectRateHistory>(x));
                    }
                    else
                    {
                        ProjectRateHistory updatedItem = _mapper.Map<ProjectRateHistory>(x);
                        updatedItem.ProjectRateHistoryId = existingItem.ProjectRateHistoryId;
                        existingProjectRateHistories.Add(updatedItem);
                        rateChangedProjectRateIds.Add(updatedItem.ProjectRateId);
                    }
                });
                
                await _projectRateHistoryRepository.UpdateRange(existingProjectRateHistories);
                await _projectRateHistoryRepository.UpdateRange(addedProjectRateHistories);
                await _projectRateHistoryRepository.SaveChangesAsync(cancellationToken);
                await MakeSureProjectValuesRecalculated(rateChangedProjectRateIds, cancellationToken);
                if (missingProjectRatePRHistories.Any())
                    NotifyMissingProjectRates(missingProjectRatePRHistories);
            }
            catch (CustomExceptions.CustomException ex)
            {
                _dataSyncLogger.LogWarning($"{GetType().Name} - SyncProcess skipped invalid data: {ex.Message}");
            }
        }

        //Forcing Project Value recalculation for Projects which have updated rates.
        private async Task MakeSureProjectValuesRecalculated(List<int> rateChangedProjectRateIds , CancellationToken cancellationToken)
        {
            if (!rateChangedProjectRateIds.Any() || cancellationToken.IsCancellationRequested) return;
            var projectRates = await _projectRateRepository.GetAsync(x => rateChangedProjectRateIds.Contains(x.ProjectRateId) , cancellationToken);
            List<int> projectIds = projectRates.Select(x => x.ProjectId).ToList();
            await _projectRepository.ClearProjectValue(projectIds);
        }

        private void NotifyMissingProjectRates(IEnumerable<Outbound.ProjectRateHistory> missingProjectRatePRHistories)
        {
            var missingItems = new Dictionary<int, string>();
            missingProjectRatePRHistories.ToList().ForEach(
                x =>
                {
                    missingItems.Add(x.RateHistoryId, $"{x.RateId}/{x.RateName}");
                });
            var msg = String.Format("{0}: {1}", "ProjectRateHistories without existing ProjectRates", missingItems.ToJsonString());
            throw new CustomExceptions.InvalidDataException(msg);
        }
    }
}
