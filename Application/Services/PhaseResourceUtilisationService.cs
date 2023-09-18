using AutoMapper;
using ForecastingSystem.Application.Common;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Services
{
    public class PhaseResourceUtilisationService : IPhaseResourceUtilisationService
    {
        private readonly IPhaseResourceUtilisationRepository _phaseResourceUtilisationRepository;
        private readonly IProjectPhaseRepository _projectPhaseRepository;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider; // use this to prevent circular DI error
        public PhaseResourceUtilisationService(IMapper mapper,
            IPhaseResourceUtilisationRepository phaseResourceUtilisationRepository,
            IProjectPhaseRepository projectPhaseRepository,
            IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _phaseResourceUtilisationRepository = phaseResourceUtilisationRepository;
            _projectPhaseRepository = projectPhaseRepository;
            _serviceProvider = serviceProvider;
        }
        public async Task GeneratePhaseResourceUtilisations(int phaseId)
        {
            var phaseResources = await _projectPhaseRepository.GetPhaseResourcesToCalculateUtilisation(phaseId);
            var _projectPhaseService = _serviceProvider.GetService<IProjectPhaseService>();
            var phaseRevenue = await _projectPhaseService.GetPhaseRevenue(phaseId);

            if (!phaseResources.Any())
            {
                await _projectPhaseService.SavePhaseRevenueValueAsync(phaseRevenue);
                return;
            }

            // delete existing utilisation
            var utilisationsToDelete = await DeletePhaseResourceUtilisations(phaseId);

            // generate/regenrate the utilisation
            await _projectPhaseService.SavePhaseRevenueValueAsync(phaseRevenue);
            
            var utilisationsToAdd = new List<PhaseResourceUtilisation>();
            if (phaseRevenue.EstimatedEndDate.HasValue)
            {
                foreach (var resource in phaseResources)
                {
                    var newUtilisations = new List<PhaseResourceUtilisation>();

                    var exceptionHours = resource.PhaseResourceExceptions.Select(s => new
                    {
                        s.StartWeek,
                        EndWeek = s.NumberOfWeeks == 1 ? s.StartWeek : s.StartWeek.AddDays((s.NumberOfWeeks - 1) * 7),
                        s.HoursPerWeek
                    });

                    var startDate = resource.Phase.StartDate.Value.CurrentWeekMonday();
                    var endDate = phaseRevenue.EstimatedEndDate.Value.CurrentWeekMonday();
                    while (startDate <= endDate)
                    {
                        var exception = exceptionHours.FirstOrDefault(s => s.StartWeek <= startDate && startDate <= s.EndWeek);
                        var byWeek = new PhaseResourceUtilisation()
                        {
                            PhaseId = phaseId,
                            PhaseResourceId = resource.PhaseResourceId,
                            StartWeek = startDate,
                            TotalHours = exception != null ? exception.HoursPerWeek : (float)resource.HoursPerWeek
                        };
                        utilisationsToAdd.Add(byWeek);
                        startDate = startDate.NextWeekMonday();
                    }
                }

                if (utilisationsToAdd.Any())
                    await _phaseResourceUtilisationRepository.UpdateRange(utilisationsToAdd);
            }

            if (utilisationsToDelete.Any() || utilisationsToAdd.Any())
                await _phaseResourceUtilisationRepository.SaveChangesAsync();

            // recalculate phasevalue and estimated date for linked project
            if (phaseResources.First().Phase.TimesheetPhaseId.HasValue)
            {
                var projectPhase = await _projectPhaseRepository.GetProjectPhaseByExternalPhaseId(phaseResources.First().Phase.TimesheetPhaseId.Value);
                if (projectPhase != null)
                {
                    var projectPhaseRevenue = await _projectPhaseService.GetPhaseRevenue(projectPhase.PhaseId);
                    await _projectPhaseService.SavePhaseRevenueValueAsync(projectPhaseRevenue);
                }
            }
        }

        public async Task<IEnumerable<PhaseResourceUtilisation>> DeletePhaseResourceUtilisations(int phaseId)
        {
            var utilisationsToDelete = await _phaseResourceUtilisationRepository.GetAsync(s => s.PhaseId == phaseId);
            if (utilisationsToDelete.Any())
                await _phaseResourceUtilisationRepository.DeleteRangeAsync(utilisationsToDelete);
            return utilisationsToDelete;
        }
    }
}
