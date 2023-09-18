using AutoMapper;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Common;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Services
{
    public partial class ProjectPhaseService : IProjectPhaseService
    {
        private readonly IProjectPhaseRepository _projectPhaseRepository;
        private readonly IPublicHolidayRepository _publicHolidayRepository;
        private readonly IEmployeeLeaveRepository _employeeLeaveRepository;
        private readonly IProjectRevenueCalculatorService _projectRevenueCalculatorService;
        private readonly IUserIdLookupRepository _userIdLookupRepository;
        private readonly ITimesheetService _timesheetService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProjectRepository _projectRepository;
        private readonly IPhaseResourceUtilisationService _phaseResourceUtilisationService;
        private readonly IMapper _mapper;
        private readonly IPhaseResourceRepository _phaseResourceRepository;
        private readonly IPhaseSkillsetRepository _phaseSkillsetRepository;
        private readonly IProjectRateRepository _projectRateRepository;
        private readonly IPhaseResourceExceptionRepository _phaseResourceExceptionRepository;

        public ProjectPhaseService(IProjectPhaseRepository projectPhaseRepository,
            IMapper mapper,
            IPublicHolidayRepository publicHolidayRepository,
            IEmployeeLeaveRepository employeeLeaveRepository,
            IProjectRevenueCalculatorService projectRevenueCalculatorService,
            IUserIdLookupRepository userIdLookupRepository,
            ITimesheetService timesheetService,
            ICurrentUserService currentUserService,
            IProjectRepository projectRepository,
            IPhaseResourceUtilisationService phaseResourceUtilisationService,
            IPhaseResourceRepository phaseResourceRepository,
            IPhaseSkillsetRepository phaseSkillsetRepository,
            IProjectRateRepository projectRateRepository,
            IPhaseResourceExceptionRepository phaseResourceExceptionRepository)
        {
            _mapper = mapper;
            _projectPhaseRepository = projectPhaseRepository;
            _publicHolidayRepository = publicHolidayRepository;
            _employeeLeaveRepository = employeeLeaveRepository;
            _projectRevenueCalculatorService = projectRevenueCalculatorService;
            _timesheetService = timesheetService;
            _currentUserService = currentUserService;
            _projectRepository = projectRepository;
            _userIdLookupRepository = userIdLookupRepository;
            _phaseResourceUtilisationService = phaseResourceUtilisationService;
            _phaseResourceRepository = phaseResourceRepository;
            _phaseSkillsetRepository = phaseSkillsetRepository;
            _projectRateRepository = projectRateRepository;
            _phaseResourceExceptionRepository = phaseResourceExceptionRepository;
        }

        public async Task<bool> CheckCanLinkToTimeSheetPhaseId(int phaseId, int timeSheetPhaseId)
        {
            var projectRatesOfPhaseId = await _projectRateRepository.GetProjectRatesFromResouceAssignmentsByPhaseId(phaseId);
            var projectRatesOfTimeSheetPhaseId = await _projectRateRepository.GetProjectRatesByExternalPhaseId(timeSheetPhaseId);
            if (projectRatesOfTimeSheetPhaseId.Count == 0 && projectRatesOfPhaseId.Count == 0)
            {
                return true;
            }

            return projectRatesOfPhaseId.Select(x => x.RateName).All(x => projectRatesOfTimeSheetPhaseId.Select(p => p.RateName).Contains(x));
        }

        public async Task<ProjectPhaseModel> AddAsync(ProjectPhaseModel projectPhaseRequest)
        {
            AutoSetToBugetFromResouceIfHasTimesheetPhaseId(projectPhaseRequest);

            await ValidateForLinkingToTimeSheetProjectPhase(projectPhaseRequest);

            var projectPhase = _mapper.Map<Phase>(projectPhaseRequest);
            if (projectPhaseRequest.PhaseSkillsets.Any())
            {
                var entities = _mapper.Map<List<PhaseSkillset>>(projectPhaseRequest.PhaseSkillsets);
                projectPhase.PhaseSkillsets = entities;
            }

            projectPhase.Status = PhaseStatus.Active;

            await UpdatePhaseRequestBudgetFromProjectPhase(projectPhaseRequest);
            projectPhase.Budget = projectPhaseRequest.Budget;

            var addedProjectPhase = await _projectPhaseRepository.AddAsync(projectPhase);

            await _projectPhaseRepository.SaveChangesAsync();

            var result = await GetProjectPhaseByIdAsync(addedProjectPhase.PhaseId);
            await UpdatePhaseValueAndEstimatedDate(result);

            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var projectPhase = await _projectPhaseRepository.GetByIdAsync(id);

            if (projectPhase != null)
            {
                projectPhase.Status = PhaseStatus.Inactive;
                await _phaseResourceUtilisationService.DeletePhaseResourceUtilisations(id);

                await _projectPhaseRepository.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<ProjectPhaseModel> EditAsync(ProjectPhaseModel projectPhaseRequest)
        {
            var existingItem = await _projectPhaseRepository.GetProjectPhaseByIdAsync(projectPhaseRequest.PhaseId) ?? throw new Exception($"Phase with Id {projectPhaseRequest.PhaseId} does not exist.");
            var existingTimeSheetId = existingItem.TimesheetPhaseId;
            bool regeneratePhaseResourceUtilisation = (existingItem.StartDate.HasValue &&
                                                        projectPhaseRequest.StartDate.HasValue &&
                                                        existingItem.StartDate.Value.Date != projectPhaseRequest.StartDate.Value.Date) ||
                                                        existingItem.Budget != projectPhaseRequest.Budget;
            var isOpportunity = await _projectRepository.IsOpportunityAsync(projectPhaseRequest.ProjectId);

            if (isOpportunity)
            {
                var existingPhaseId = existingItem.PhaseId;
                var existingTimesheetPhaseId = existingItem.TimesheetPhaseId;
                AutoSetToBugetFromResouceIfHasTimesheetPhaseId(projectPhaseRequest);

                await ValidateForLinkingToTimeSheetProjectPhase(projectPhaseRequest, existingItem);

                await UpdatePhaseRequestBudgetFromProjectPhase(projectPhaseRequest, existingItem.TimesheetPhaseId);

                _mapper.Map(projectPhaseRequest, existingItem);

                existingItem.PhaseSkillsets = UpdateSkillSets(projectPhaseRequest.PhaseSkillsets?.ToList(), existingItem.PhaseSkillsets?.ToList());

                await _projectPhaseRepository.UpdateAsync(existingItem);

                await _projectPhaseRepository.SaveChangesAsync();

                await InsertResourceAssignmentsAndResourceAssignmentExceptionsToProjectPhase(projectPhaseRequest, existingPhaseId, existingTimesheetPhaseId);
            }
            else
            {
                if (existingItem.Budget != projectPhaseRequest.Budget && projectPhaseRequest.Budget.HasValue)
                {
                    await UpdateTimesheetPhaseBudgetAsync(existingItem.ExternalPhaseId, projectPhaseRequest.Budget.Value);
                }

                existingItem.PhaseSkillsets = UpdateSkillSets(projectPhaseRequest.PhaseSkillsets?.ToList(), existingItem.PhaseSkillsets?.ToList()); ;

                existingItem.Budget = projectPhaseRequest.Budget;
                await _projectPhaseRepository.UpdateAsync(existingItem);
                await _projectPhaseRepository.SaveChangesAsync();
            }

            if (regeneratePhaseResourceUtilisation)
            {
                await _phaseResourceUtilisationService.GeneratePhaseResourceUtilisations(existingItem.PhaseId);
            }

            var result = await GetProjectPhaseByIdAsync(existingItem.PhaseId);
            await UpdatePhaseValueAndEstimatedDate(result);

            if (projectPhaseRequest.TimesheetPhaseId.HasValue && projectPhaseRequest.TimesheetPhaseId != existingTimeSheetId)
            {
                await SavePhaseAndProjectValueForLinkedProjectPhase(projectPhaseRequest.TimesheetPhaseId.Value);
            }

            return result;
        }

        private async Task UpdatePhaseValueAndEstimatedDate(ProjectPhaseModel model)
        {
            if (model != null)
            {
                var revenue = await GetPhaseRevenue(model.PhaseId);
                model.PhaseValue = revenue.PhaseValue;
                model.EstimatedEndDate = revenue.EstimatedEndDate;
                model.Budget = revenue.Budget;

                await SavePhaseAndProjectValueAsync(model);
            }
        }

        private async Task UpdateTimesheetPhaseBudgetAsync(int? timesheetPhaseId, decimal budget)
        {
            if (timesheetPhaseId.HasValue)
            {
                var updatePhaseBudget = new UpdatePhaseBudgetModel
                {
                    Budget = budget,
                    PhaseID = timesheetPhaseId.Value,
                    UserName = _currentUserService.Username
                };

                try
                {
                    await _timesheetService.UpdatePhaseBudgetAsync(updatePhaseBudget);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to update Timesheet phase budget.", ex);
                }
            }
        }

        public async Task<ProjectPhaseModel> GetProjectPhaseByIdAsync(int id)
        {
            var projectPhase = await _projectPhaseRepository.GetProjectPhaseByIdAsync(id);
            if (projectPhase == null)
                return null;

            var project = await _projectRepository.FirstOrDefaultAsync(x => x.ProjectId == projectPhase.ProjectId);
            var result = _mapper.Map<ProjectPhaseModel>(projectPhase);

            if (projectPhase.ExternalPhaseId.HasValue)
            {
                result.TimesheetPhaseId = projectPhase.ExternalPhaseId.Value;
            }

            if (!string.IsNullOrEmpty(project.ProjectCode))
            {
                var records = await _projectPhaseRepository.GetTimesheetPhaseIdListByProjectCodeAsync(project.ProjectCode);
                result.TimesheetPhaseCodeNames = records.Select(x => new PhaseCodeWithNameModel
                {
                    Name = x.Key,
                    TimesheetPhaseId = x.Value
                }).ToList();
            }

            // if phase has link to 1 budget opportunity phase of then will get skill Sets of that budget opportunity phase
            if (project.ProjectType.Equals(Constants.ProjectType.Project)
                && !(projectPhase.IsCalculatingByResource ?? false)
                && projectPhase.ExternalPhaseId.HasValue)
            {
                var skillSets = await _phaseSkillsetRepository.GetAsync(x => x.Phase.TimesheetPhaseId == projectPhase.ExternalPhaseId, c => c.Skillset);
                result.LinkedOpportunityPhaseSkillsets = skillSets.Select(_mapper.Map<PhaseSkillsetModelToView>);
            }

            return result;
        }

        public ProjectPhasesListModel GetProjectPhases(int projectId)
        {
            var phases = _projectPhaseRepository.GetAsync(x => x.ProjectId == projectId).Result;
            var projectPhases = _mapper.Map<IEnumerable<ProjectPhaseModel>>(phases);

            return new ProjectPhasesListModel()
            {
                ProjectPhases = projectPhases
            };
        }

        public async Task<PhaseRevenueModel> GetPhaseRevenue(int phaseId)
        {
            var phase = await _projectPhaseRepository.GetPhaseToCalculateRevenue(phaseId);
            var holidays = await _publicHolidayRepository.GetPublicHolidaysAsync(phase.StartDate);
            var resourceEmails = phase.PhaseResources.Where(s => s.Employee != null).Select(s => s.Employee.Email).ToList();
            var userIdsLookup = await _userIdLookupRepository.GetByBambooHREmailsAsync(resourceEmails);
            var timesheetUsernames = userIdsLookup.Select(s => s.TimesheetUserName).ToList();
            var leaves = await _employeeLeaveRepository.GetLeavesAsync(timesheetUsernames: timesheetUsernames);
            var phaseRevenue = _projectRevenueCalculatorService.GetPhaseRevenue(phase, holidays, leaves, userIdsLookup);
            return phaseRevenue;
        }

        public async Task SavePhaseAndProjectValueAsync(ProjectPhaseModel projectPhaseRequest)
        {
            await _projectPhaseRepository.UpdatePhaseValues(projectPhaseRequest.PhaseId, (decimal)projectPhaseRequest.PhaseValue,
                                                                    projectPhaseRequest.Budget, projectPhaseRequest.EstimatedEndDate);
            //Set flag to enforce recalculate ProjectValue
            await _projectRepository.ClearProjectValue(projectPhaseRequest.ProjectId);
        }

        public async Task SavePhaseRevenueValueAsync(PhaseRevenueModel phaseRevenue)
        {
            var dbPhase = await _projectPhaseRepository.GetProjectPhaseByIdAsync(phaseRevenue.PhaseId) ?? throw new Exception($"Phase with Id {phaseRevenue.PhaseId} does not exist.");
            dbPhase.Budget = phaseRevenue.Budget;
            dbPhase.PhaseValue = (decimal)phaseRevenue.PhaseValue;
            dbPhase.EstimatedEndDate = phaseRevenue.EstimatedEndDate;

            await _projectPhaseRepository.UpdateAsync(dbPhase);
            await _projectPhaseRepository.SaveChangesAsync();

            //Set flag to enforce recalculate ProjectValue
            var projectDb = await _projectRepository.GetByIdAsync(dbPhase.ProjectId);
            projectDb.IsObsoleteProjectValue = true;
            await _projectRepository.UpdateAsync(projectDb);
            await _projectRepository.SaveChangesAsync();
        }

        private void AutoSetToBugetFromResouceIfHasTimesheetPhaseId(ProjectPhaseModel projectPhaseRequest)
        {
            if (projectPhaseRequest.TimesheetPhaseId.HasValue
               && projectPhaseRequest.IsCalculatingByResource == true)
            {
                projectPhaseRequest.IsCalculatingByResource = false;
            }
        }

        private async Task ValidateForLinkingToTimeSheetProjectPhase(
            ProjectPhaseModel projectPhaseRequest,
            Phase existingOpportunityPhase = null)
        {
            if (!projectPhaseRequest.TimesheetPhaseId.HasValue)
            {
                return;
            }

            if (existingOpportunityPhase != null
                && projectPhaseRequest.TimesheetPhaseId != existingOpportunityPhase.TimesheetPhaseId
                && await _projectPhaseRepository.AnyAsync(x => x.Status == PhaseStatus.Active && x.TimesheetPhaseId == projectPhaseRequest.TimesheetPhaseId && x.PhaseId != projectPhaseRequest.PhaseId))
            {
                var phase = await _projectPhaseRepository.FirstOrDefaultAsync(x => x.Status == PhaseStatus.Active && x.ExternalPhaseId == projectPhaseRequest.TimesheetPhaseId);
                throw new BussinessException($"Timesheet Phase ID '[{phase.PhaseCode} - {phase.PhaseName}]' is linked to another phase.");
            }

            if ((existingOpportunityPhase != null
                    && projectPhaseRequest.IsCalculatingByResource == true
                    && existingOpportunityPhase.IsCalculatingByResource == false)
                || (existingOpportunityPhase == null
                    && projectPhaseRequest.IsCalculatingByResource == true))
            {
                throw new BussinessException("Not allow to change from By Budget to By Resource.");
            }
        }

        private async Task InsertResourceAssignmentsAndResourceAssignmentExceptionsToProjectPhase(ProjectPhaseModel projectPhaseRequest, int exitingPhaseId, int? existingTimesheetPhaseId)
        {
            if (!projectPhaseRequest.TimesheetPhaseId.HasValue
                || projectPhaseRequest.TimesheetPhaseId.Value == existingTimesheetPhaseId)
            {
                return;
            }

            var projectRatesOfTimeSheetPhaseId = await _projectRateRepository.GetProjectRatesByExternalPhaseId(projectPhaseRequest.TimesheetPhaseId.Value);
            var phaseResources = await _phaseResourceRepository.GetFullAsync(x => x.PhaseId == exitingPhaseId);
            if (!phaseResources.Any())
            {
                return;
            }

            var phaseProject = await _projectPhaseRepository.GetProjectPhaseByExternalPhaseId(projectPhaseRequest.TimesheetPhaseId.Value);
            var newPhaseResourceExceptions = new List<PhaseResourceException>();

            foreach (var phaseResource in phaseResources)
            {
                var projectRateId = projectRatesOfTimeSheetPhaseId.First(x => x.RateName.Equals(phaseResource.ProjectRate.RateName)).ProjectRateId;
                var newPhaseResource = new PhaseResource
                {
                    EmployeeId = phaseResource.EmployeeId,
                    PhaseId = phaseProject.PhaseId,
                    HoursPerWeek = phaseResource.HoursPerWeek,
                    FTE = phaseResource.FTE,
                    ProjectRateId = projectRateId,
                };
                if (phaseResource.ResourcePlaceHolderId.HasValue)
                {
                    newPhaseResource.ResourcePlaceHolder = new ResourcePlaceHolder
                    {
                        ResourcePlaceHolderName = phaseResource.ResourcePlaceHolder.ResourcePlaceHolderName,
                        Country = phaseResource.ResourcePlaceHolder.Country
                    };
                }

                var result = await _phaseResourceRepository.AddPhaseResourceAsync(newPhaseResource);

                foreach (var phaseResourceException in phaseResource.PhaseResourceExceptions)
                {
                    newPhaseResourceExceptions.Add(new PhaseResourceException
                    {
                        HoursPerWeek = phaseResourceException.HoursPerWeek,
                        NumberOfWeeks = phaseResourceException.NumberOfWeeks,
                        StartWeek = phaseResourceException.StartWeek,
                        PhaseResourceId = result.PhaseResourceId,
                    });
                }
            }

            if (newPhaseResourceExceptions.Any())
            {

                await _phaseResourceExceptionRepository.UpdateRange(newPhaseResourceExceptions);
                await _phaseResourceRepository.SaveChangesAsync();
            }

            await _phaseResourceUtilisationService.GeneratePhaseResourceUtilisations(phaseProject.PhaseId);
        }

        private async Task UpdatePhaseRequestBudgetFromProjectPhase(ProjectPhaseModel projectPhaseRequest, int? existingTimesheetPhaseId = null)
        {
            if (!projectPhaseRequest.TimesheetPhaseId.HasValue
                || projectPhaseRequest.TimesheetPhaseId == existingTimesheetPhaseId)
            {
                return;
            }

            var projectPhase = await _projectPhaseRepository.GetProjectPhaseByExternalPhaseId(projectPhaseRequest.TimesheetPhaseId.Value);
            if (projectPhase == null)
            {
                return;
            }

            projectPhaseRequest.PhaseName = projectPhase.PhaseName;

            if (projectPhase.Budget.HasValue && projectPhase.Budget > 0)
            {
                projectPhaseRequest.Budget = projectPhase.Budget;
            }
            else if (projectPhaseRequest.Budget.HasValue && projectPhaseRequest.Budget > 0)
            {
                projectPhase.Budget = projectPhaseRequest.Budget;
                await UpdateTimesheetPhaseBudgetAsync(projectPhaseRequest.TimesheetPhaseId.Value, projectPhaseRequest.Budget.Value);
                await _projectPhaseRepository.UpdateAsync(projectPhase);
            }
        }

        private IList<PhaseSkillset> UpdateSkillSets(IList<PhaseSkillsetModelToView> phaseSkillsets, IList<PhaseSkillset> existingPhaseSkillsets)
        {
            if (phaseSkillsets.Any())
            {
                var newItems = phaseSkillsets.Where(x => x.PhaseSkillSetId == 0)
                    .Select(y => new PhaseSkillset() { PhaseId = y.PhaseId, SkillsetId = y.SkillsetId });
                var updatedItems = phaseSkillsets.Where(x => x.PhaseSkillSetId > 0);
                var deletedItems = existingPhaseSkillsets.Where(x => !phaseSkillsets.Select(y => y.PhaseSkillSetId).Contains(x.PhaseSkillSetId));
                newItems.ToList().ForEach(x =>
                {
                    existingPhaseSkillsets.Add(x);
                });
                updatedItems.ToList().ForEach(x =>
                {
                    var existingSkillset = existingPhaseSkillsets.First(y => y.PhaseSkillSetId == x.PhaseSkillSetId);
                    _mapper.Map(x, existingSkillset);
                });

                // PhaseId and SkillsetId are not null so cannot delete by Remove method.
                if (deletedItems.Any())
                {
                    _projectPhaseRepository.DeleteSkillsets(deletedItems);
                }
            }
            else if (existingPhaseSkillsets.Any())
            {
                // PhaseId and SkillsetId are not null so cannot delete by Remove method.
                _projectPhaseRepository.DeleteSkillsets(existingPhaseSkillsets);
            }

            return existingPhaseSkillsets;
        }

        private async Task SavePhaseAndProjectValueForLinkedProjectPhase(int timesheetPhaseId)
        {
            var linkProjectPhase = await _projectPhaseRepository.FirstOrDefaultAsync(x => x.ExternalPhaseId == timesheetPhaseId);
            var revenue = await GetPhaseRevenue(linkProjectPhase.PhaseId);

            await _projectPhaseRepository.UpdatePhaseValues(revenue.PhaseId, (decimal)revenue.PhaseValue,
                                                                    revenue.Budget, revenue.EstimatedEndDate);
            //Set flag to enforce recalculate ProjectValue
            await _projectRepository.ClearProjectValue(linkProjectPhase.ProjectId);
        }
    }
}
