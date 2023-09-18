using AutoMapper;
using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.DataSyncServices.DataAccessServices
{
    public class PhaseService : IPhaseService
    {
        private readonly ISyncProjectRepository _projectRepository;
        private readonly ISyncProjectPhaseRepository _phaseRepository;
        private readonly IDataSyncServiceLogger _dataSyncLogger;
        private readonly IMapper _mapper;

        public PhaseService(ISyncProjectRepository projectRepository
            , ISyncProjectPhaseRepository phaseRepository
            , IDataSyncServiceLogger dataSyncLogger
            , IMapper mapper)
        {
            _projectRepository = projectRepository;
            _phaseRepository = phaseRepository;
            _dataSyncLogger = dataSyncLogger;
            _mapper = mapper;
        }

        public async Task AddOrUpdateRangeAsync(IEnumerable<Outbound.Phase> tsPhases , CancellationToken cancellationToken)
        {
            try
            {
                var externalProjectIds = tsPhases.Select(x => x.ProjectId).ToList();
                var projects = await _projectRepository.GetAsync(x => externalProjectIds.Contains(x.ExternalProjectId ?? 0) , cancellationToken);
                var missingProjectPhases = new List<Outbound.Phase>();
                tsPhases.ToList().ForEach(x =>
                {
                    var project = projects.FirstOrDefault(y => y.ExternalProjectId == x.ProjectId);
                    if (project == null)
                    {
                        missingProjectPhases.Add(x);
                        return;
                    }

                    x.FSProjectId = project.ProjectId;
                });
                tsPhases = tsPhases.Where(x => x.FSProjectId > 0);
                var allPhases = await _phaseRepository.GetAllAsync(cancellationToken);

                var existingPhases = new List<Phase>();
                var addedPhases = new List<Phase>();
                tsPhases.ToList().ForEach(x =>
                {
                    if (x.EndDate == new DateTime(1900, 01, 01)) x.EndDate = null;

                    var existingItem = allPhases.FirstOrDefault(t => t.ExternalPhaseId == x.PhaseId);
                    if (existingItem == null)
                    {
                        addedPhases.Add(_mapper.Map<Phase>(x));
                    }
                    else
                    {
                        Phase updatedItem = _mapper.Map<Phase>(x);
                        updatedItem.PhaseId = existingItem.PhaseId;
                        existingPhases.Add(updatedItem);
                    }
                });

                await _phaseRepository.UpdateRange(existingPhases);
                await _phaseRepository.UpdateRange(addedPhases);
                await _phaseRepository.SaveChangesAsync(cancellationToken);
                if (missingProjectPhases.Any())
                    NotifyMissingProjects(missingProjectPhases);
            }
            catch (CustomExceptions.CustomException ex)
            {
                _dataSyncLogger.LogWarning($"{GetType().Name} - SyncProcess skipped invalid data: {ex.Message}");
            }
        }

        private void NotifyMissingProjects(IEnumerable<Outbound.Phase> missingProjectPhases)
        {
            var missingItems = new Dictionary<int , string>();
            missingProjectPhases.ToList().ForEach(
                x =>
                {
                    missingItems.Add(x.PhaseId , $"{x.ProjectId}/{x.ProjectName}");
                });
            var msg = String.Format("{0}: {1}" , "Phases without existing projects" , missingItems.ToJsonString());
            throw new CustomExceptions.InvalidDataException(msg);
        }
    }
}
