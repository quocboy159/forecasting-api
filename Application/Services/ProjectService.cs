using AutoMapper;
using ForecastingSystem.Application.Common;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Common;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProjectRateRepository _projectRateRepository;
        private readonly IProjectRateHistoryRepository _projectRateHistoryRepository;
        private readonly IProjectPhaseRepository _projectPhaseRepository;
        private readonly IProjectRateService _projectRateService;
        private readonly IPublicHolidayRepository _publicHolidayRepository;
        private readonly IEmployeeLeaveRepository _employeeLeaveRepository;
        private readonly IProjectRevenueCalculatorService _projectRevenueCalculatorService;
        private readonly IUserIdLookupRepository _userIdLookupRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmployeeTimesheetEntryRepository _employeeTimesheetEntryRepository;
        private readonly IMapper _mapper;
        private readonly IProjectEmployeeManagerRepository _projectEmployeeManagerRepository;

        public ProjectService(IMapper mapper,
                              IProjectRepository projectRepository,
                              IClientRepository clientRepository,
                              IProjectRateRepository projectRateRepository,
                              IProjectRateHistoryRepository projectRateHistoryRepository,
                              IProjectPhaseRepository projectPhaseRepository,
                              IProjectRateService projectRateService,
                              IPublicHolidayRepository publicHolidayRepository,
                              IEmployeeLeaveRepository employeeLeaveRepository,
                              IProjectRevenueCalculatorService projectRevenueCalculatorService,
                              IUserIdLookupRepository userIdLookupRepository,
                              ICurrentUserService currentUserService,
                              IEmployeeTimesheetEntryRepository employeeTimesheetEntryRepository,
                              IProjectEmployeeManagerRepository projectEmployeeManagerRepository)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
            _clientRepository = clientRepository;
            _projectRateRepository = projectRateRepository;
            _projectRateHistoryRepository = projectRateHistoryRepository;
            _projectPhaseRepository = projectPhaseRepository;
            _projectRateService = projectRateService;
            _publicHolidayRepository = publicHolidayRepository;
            _employeeLeaveRepository = employeeLeaveRepository;
            _projectRevenueCalculatorService = projectRevenueCalculatorService;
            _userIdLookupRepository = userIdLookupRepository;
            _currentUserService = currentUserService;
            _employeeTimesheetEntryRepository = employeeTimesheetEntryRepository;
            _projectEmployeeManagerRepository = projectEmployeeManagerRepository;
        }

        public async Task<bool> CheckProjectHasLinkedPhase(int projectId)
        {
            return await _projectPhaseRepository
                    .AnyAsync(x => x.ProjectId == projectId
                                   && x.Status == PhaseStatus.Active
                                   && x.TimesheetPhaseId.HasValue);
        }

        public async Task<Project> SaveAsync(ProjectDetailAddEditModel projectRequest)
        {
            var project = _mapper.Map<Project>(projectRequest);
            if (project.ClientId != 0)
            {
                project.Client = await _clientRepository.GetByIdAsync(project.ClientId);
            }

            bool ratesHasUpdated = false;
            if (projectRequest.ProjectId == 0)
            {
                project.ProjectId = 0;
                project.ProjectType = Constants.ProjectType.Opportunity;
                project.ProjectRates = new List<ProjectRate>();

                if (project.Phases.FirstOrDefault() != null)
                {
                    project.Phases.First().Status = PhaseStatus.Active;
                    project.Phases.First().EndDate = projectRequest.Phases.First().EndDate;
                }

                foreach (var rateItem in projectRequest.Rates)
                {
                    var projectRate = new ProjectRate()
                    {
                        RateName = rateItem.RateName,
                        ProjectRateHistories = new List<ProjectRateHistory>()
                    {
                        new ProjectRateHistory(){ StartDate = rateItem.EffectiveDate, Rate = (double) rateItem.HourlyRate}
                    }
                    };

                    project.ProjectRates.Add(projectRate);
                }

                await _projectRepository.AddAsync(project);
                await _projectRepository.SaveChangesAsync();

                await UpdateProjectManagers(projectRequest.ProjectManagerIds, project);

                return project;
            }
            else
            {
                var rawProject = (await _projectRepository.GetAsync(x => x.ProjectId == projectRequest.ProjectId)).First();

                if (rawProject.ProjectType != Constants.ProjectType.Project)
                {
                    if (await CheckExistingLinkedPhase(projectRequest, rawProject))
                    {
                        throw new BussinessException("The opportunity contains phases already linked to the project phase, please remove the linked phases.");
                    }

                    // Not allow to modified here
                    project.Phases = await _projectPhaseRepository.GetListByProjectIdAsync(project.ProjectId);
                    project.ProjectType = rawProject.ProjectType;

                    project.ProjectRates = project.ProjectRates.Where(x => x.ProjectRateId != 0).ToList();

                    // Add or edit rate history
                    foreach (var rateItem in projectRequest.Rates)
                    {
                        if (rateItem.ProjectRateId == 0)
                        {
                            var projectRate = new ProjectRate()
                            {
                                RateName = rateItem.RateName,
                                ProjectRateHistories = new List<ProjectRateHistory>()
                                {
                                    new ProjectRateHistory(){ StartDate = rateItem.EffectiveDate, Rate = (double) rateItem.HourlyRate}
                                }
                            };

                            project.ProjectRates.Add(projectRate);
                        }
                        else
                        {
                            ratesHasUpdated = true;
                            var projectRate = project.ProjectRates.First(x => x.ProjectRateId == rateItem.ProjectRateId);

                            var recentRateForSpecificDate = await _projectRateService.GetMostRecentRateValueForSepecificDateAsync(rateItem.ProjectRateId, rateItem.EffectiveDate.Value);
                            if (recentRateForSpecificDate != null)
                            {
                                if (recentRateForSpecificDate.Rate != (double)rateItem.HourlyRate)
                                {
                                    projectRate.ProjectRateHistories.Add(
                                        new ProjectRateHistory() { StartDate = rateItem.EffectiveDate, Rate = (double)rateItem.HourlyRate }
                                    );
                                }
                            }
                            else
                            {
                                projectRate.ProjectRateHistories.Add(
                                    new ProjectRateHistory() { StartDate = rateItem.EffectiveDate, Rate = (double)rateItem.HourlyRate }
                                );
                            }
                        }
                    }

                    // Remove rate not in list
                    var rateIds = projectRequest.Rates.Select(q => q.ProjectRateId).ToList();
                    var deleteRates = (await _projectRateRepository.GetAsync(x => x.ProjectId == project.ProjectId && !rateIds.Contains(x.ProjectRateId))).ToList();
                    foreach (var deleteRate in deleteRates)
                    {
                        deleteRate.Status = ProjectRateStatus.Inactive;
                        await _projectRateRepository.UpdateAsync(deleteRate);
                    }

                    if (ratesHasUpdated)
                    {
                        project.IsObsoleteProjectValue = true;
                        foreach (var phase in project.Phases)
                        {
                            phase.PhaseValue = null;

                        }
                    }

                    await _projectRepository.UpdateAsync(project);
                    await _projectRepository.SaveChangesAsync();

                    await UpdateProjectManagers(projectRequest.ProjectManagerIds, project);

                    //await _projectRepository.SaveChangesAndRefreshAsync();


                }

                return project;
            }
        }

        public async Task<ProjectModel> GetProjectByIdAsync(int projectId)
        {
            var projectDb = await _projectRepository.FirstOrDefaultAsync(x => x.ProjectId == projectId);
            return _mapper.Map<ProjectModel>(projectDb);
        }

        public async Task<ProjectListModel> GetAllProjectsAsync()
        {
            IEnumerable<Project> dbProjects;
            var userRole = await _currentUserService.GetUserRoleAsync();

            if (userRole == Constants.UserRole.Admin)
            {
                dbProjects = await _projectRepository.GetAllProjectsAsync();
            }
            else if (userRole == Constants.UserRole.ProjectManager)
            {
                dbProjects = await _projectRepository.GetProjectsByUserAsync(_currentUserService.Username);
            }
            else
            {
                var emptyResult = new ProjectListModel();
                emptyResult.Projects = new List<ProjectModel>();
                return emptyResult;
            }

            var projects = _mapper.Map<IEnumerable<ProjectModel>>(dbProjects);

            foreach (var item in projects)
            {
                var project = dbProjects.First(x => x.ProjectId == item.ProjectId);
                item.ProjectBudget = project.ProjectBudget;
                item.ClientName = project.Client.ClientName;
            }

            return new ProjectListModel()
            {
                Projects = projects.OrderByDescending(x => x.UpdatedDateTime).ToList(),
            };
        }

        public async Task<ProjectDetailModel> GetProjectDetailAsync(int projectId)
        {
            var projectDb = await _projectRepository.GetProjectDetailAsync(projectId);

            var projectDetailViewModel = _mapper.Map<ProjectDetailModel>(projectDb);

            if (projectDetailViewModel != null)
            {
                projectDetailViewModel.Rates = await HandleProcessProjectRates(projectDetailViewModel.Rates);
            }

            return projectDetailViewModel;
        }

        public bool IsOpportunity(int projectId)
        {
            var project = GetProjectByIdAsync(projectId).Result;
            return project != null && project.ProjectType == Constants.ProjectType.Opportunity;
        }

        public async Task<ProjectRevenueModel> GetProjectRevenue(int projectId)
        {
            var revenues = await GetProjectRevenues(new List<int>() { projectId });
            if (!revenues.Any()) return new ProjectRevenueModel();
            var projectRevenue = revenues.FirstOrDefault();
            projectRevenue.HasChangedProjectValue = true;
            await SaveCalculatedProjectRevenues(revenues);
            return projectRevenue;
        }

        public async Task<List<ProjectRevenueModel>> GetProjectRevenues(List<int> projectIds)
        {
            var projects = await _projectRepository.GetProjectsToCalculateRevenue(projectIds: projectIds, status: ProjectStatus.Active);
            if (!projects.Any()) return new List<ProjectRevenueModel>();
            var minStartDate = projects.Min(s => s.StartDate); // only need public holiday and leave start from this MinDate
            var holidays = await _publicHolidayRepository.GetPublicHolidaysAsync(minStartDate);
            var leaves = await _employeeLeaveRepository.GetLeavesAsync(minStartDate);
            var userIdsLookup = await _userIdLookupRepository.GetAllAsync();
            var projectRevenues = _projectRevenueCalculatorService.GetProjectRevenues(projects, holidays, leaves, userIdsLookup.ToList());
            await SaveCalculatedProjectRevenues(projectRevenues);

            return projectRevenues;
        }

        private async Task SaveCalculatedProjectRevenues(List<ProjectRevenueModel> projectRevenues)
        {
            //detect changes and then save.
            foreach (var projectRevenue in projectRevenues.Where(x => x.HasChangedProjectValue).ToList())
            {
                await SaveProjectAndPhaseValues(projectRevenue);
            }
        }

        private async Task SaveProjectAndPhaseValues(ProjectRevenueModel projectRevenue)
        {
            double projectBudget = 0;
            foreach (var phaseRevenue in projectRevenue.PhaseRevenues)
            {
                projectBudget += (double)(phaseRevenue.Budget == null ? 0 : phaseRevenue.Budget);
                await SavePhaseValue(phaseRevenue);
            }

            await SaveProjectValue(projectRevenue.ProjectId, projectRevenue.ProjectValue, projectBudget);
        }

        private async Task SaveProjectValue(int projectId, float projectValue, double projectBudget)
        {
            await _projectRepository.UpdateProjectValues(projectId, (decimal)projectValue, (decimal)projectBudget);
        }

        private async Task SavePhaseValue(PhaseRevenueModel phaseRevenue)
        {
            if (!phaseRevenue.HasChangedPhaseValue) return;

            var phasedb = await _projectPhaseRepository.FirstOrDefaultAsync(x => x.PhaseId == phaseRevenue.PhaseId);
            await _projectPhaseRepository.UpdatePhaseValues(phaseRevenue.PhaseId, (decimal)phaseRevenue.PhaseValue, phaseRevenue?.Budget, phaseRevenue?.EstimatedEndDate);
        }

        public async Task<(List<ProjectRevenueModel>, List<string> consumedTimes)> GetProjectsRevenueForecast(DateTime startWeek, int maxNumberOfWeeks = 52, int maxNumberOfMonths = 12)
        {
            string timeFormat = @"m\:ss\.fff";
            List<string> consumedTimes = new();
            var timer = new Stopwatch();
            timer.Start();

            var projectIds = await GetProjectIdsByLoginUserAsync();
            var projects = await _projectRepository.GetProjectsToCalculateRevenue(projectIds, status: ProjectStatus.Active, isClosed: false, isCompleted: false, excludeLinkedOpportunityToProject: true);

            if (!projects.Any()) return (new List<ProjectRevenueModel>(), consumedTimes);
            var minStartDate = projects.Min(s => s.StartDate); // only need public holiday and leave start from this MinDate
            var holidays = await _publicHolidayRepository.GetPublicHolidaysAsync(minStartDate);
            var leaves = await _employeeLeaveRepository.GetLeavesAsync(minStartDate);
            var userIdsLookup = await _userIdLookupRepository.GetAllAsync();
            timer.Stop();
            consumedTimes.Add($"Revenue Forecast - Query data prepare to calculate: {timer.Elapsed.ToString(timeFormat)}");

            timer.Start();
            var projectRevenues = _projectRevenueCalculatorService.GetProjectRevenues(projects, holidays, leaves, userIdsLookup.ToList(), mustRecalculate: true);
            timer.Stop();
            consumedTimes.Add($"Revenue Forecast - Calculate the project revenue: {timer.Elapsed.ToString(timeFormat)}");

            // remove revenue in past week/month for Opportunity
            var currentWeek = DateTime.Now.Date.CurrentWeekMonday();
            var currentMonth = DateTime.Now.Date.FirstDayOfMonth();
            projectRevenues.ForEach(s =>
            {
                if (s.ProjectType == Constants.ProjectType.Opportunity)
                {
                    s.PhaseRevenues.ForEach(x =>
                    {
                        x.RevenueByWeeks = x.RevenueByWeeks.Where(z => z.StartDate >= currentWeek).ToList();
                        x.RevenueByMonths = x.RevenueByMonths.Where(z => z.StartDate >= currentMonth).ToList();
                    });
                }
            });

            // show up these real projects and Opportunity have revenue value on week equal or after selected startWeek
            projectRevenues = projectRevenues.Where(s => s.ProjectType == Constants.ProjectType.Project || s.LargestWeek >= startWeek).ToList();
            //show up Opportunity / project that has Project value > 0
            projectRevenues = projectRevenues.Where(s => s.ProjectValue > 0).ToList();

            // get real project to calculate the actual revenue
            var externalProjectIds = projectRevenues.Where(s => s.ProjectType == Constants.ProjectType.Project)
                                                        .Select(s => s.ExternalProjectId).ToList();
            if (startWeek < currentWeek && externalProjectIds.Any())
            {
                timer.Start();
                // calculate actual revenue for real projects in past time
                var timesheetEntries = await _employeeTimesheetEntryRepository.GetEntriesToCalculateRevenue(externalProjectIds, startWeek.FirstDayOfMonth(), currentWeek.EndOfPrevDateTime()); // get full of month for month view
                var projectActualRevenues = _projectRevenueCalculatorService.GetActualProjectRevenues(timesheetEntries, startWeek, currentWeek);
                // overwrite this actual list to final list, and set the flag IsActual
                foreach (var project in projectRevenues)
                {
                    var actualProjectPhases = projectActualRevenues.Where(s => s.ExternalProjectId == project.ExternalProjectId).ToList();
                    if (actualProjectPhases.Any())
                    {
                        foreach (var actualPhase in actualProjectPhases)
                        {
                            var phase = project.PhaseRevenues.FirstOrDefault(s => s.PhaseCode == actualPhase.PhaseCode);
                            if (phase != null)
                            {
                                //if actualPhase.RevenueByWeeks week does not exist in phase.RevenueByWeeks, update week value
                                foreach (var week in phase.RevenueByWeeks)
                                {
                                    var actualWeek = actualPhase.RevenueByWeeks.FirstOrDefault(s => s.StartDate == week.StartDate);
                                    if (actualWeek != null)
                                    {
                                        week.Revenue = actualWeek.Revenue;
                                        week.IsActual = true;
                                    }
                                }

                                //if actualPhase.RevenueByWeeks week does not exist in phase.RevenueByWeeks, then add it to  phase.RevenueByWeeks
                                foreach (var actualWeek in actualPhase.RevenueByWeeks)
                                {
                                    if(!phase.RevenueByWeeks.Any(revWeek => revWeek.StartDate == actualWeek.StartDate))
                                    {
                                        actualWeek.IsActual = true;
                                        phase.RevenueByWeeks.Add(actualWeek);
                                    }
                                }


                                foreach (var month in phase.RevenueByMonths)
                                {
                                    var actualMonth = actualPhase.RevenueByMonths.FirstOrDefault(s => s.StartDate == month.StartDate);
                                    if (actualMonth != null)
                                    {
                                        if (actualMonth.StartDate.Year == currentWeek.Year &&
                                            actualMonth.StartDate.Month == currentWeek.Month &&
                                            actualMonth.EndDate.HasValue)
                                        {
                                            // current month:
                                            // Actual: from 1st to lastweek, 
                                            // FS: from current week to end of month
                                            var revenueFromFS = month.CostPerDays.Where(s => s.Key > actualMonth.EndDate.Value).Sum(s => s.Value);
                                            month.Revenue = actualMonth.Revenue + revenueFromFS;
                                        }
                                        else
                                        {
                                            //past month
                                            month.Revenue = actualMonth.Revenue;
                                            month.IsActual = true;
                                        }
                                    }
                                }

                                //if actualPhase.RevenueByMonths month does not exist in phase.RevenueByMonths, then add it to  phase.RevenueByMonths
                                foreach (var actualMonth in actualPhase.RevenueByMonths)
                                {
                                    if (!phase.RevenueByMonths.Any(revWeek => revWeek.StartDate == actualMonth.StartDate))
                                    {
                                        actualMonth.IsActual = true;
                                        phase.RevenueByMonths.Add(actualMonth);
                                    }
                                }
                            }
                        }
                    }
                }
                timer.Stop();
                consumedTimes.Add($"Revenue Forecast - Calculate actual revenue for real projects in past time: {timer.Elapsed.ToString(timeFormat)}");
            }

            var monthEndDate = startWeek.FirstDayOfMonth().AddMonths(maxNumberOfMonths);
            var weekEndDate = startWeek.CurrentWeekMonday().AddDays(maxNumberOfWeeks * 7);

            foreach (var project in projectRevenues)
            {    
                foreach (var phase in project.PhaseRevenues)
                {
                    phase.RevenueByWeeks = phase.RevenueByWeeks.Where(s => s.StartDate >= startWeek).Take(maxNumberOfWeeks).ToList();
                    phase.RevenueByMonths = phase.RevenueByMonths.Where(s => s.StartDate >= startWeek.FirstDayOfMonth()).Take(maxNumberOfMonths).ToList();                    
                    
                    if (project.ProjectType == ProjectType.Opportunity)
                    {
                        phase.ImpactDetailsByMonths = phase.ImpactDetails.Where(s => s.MonthStartDate >= DateTime.Today.FirstDayOfMonth() && s.MonthStartDate <= monthEndDate).ToList();
                        phase.ImpactDetailsByWeeks = phase.ImpactDetails.Where(s => s.WeekStartDate >= DateTime.Today.CurrentWeekMonday() &&  s.WeekStartDate <= weekEndDate).ToList();
                    }
                    else
                    {
                        phase.ImpactDetailsByMonths = phase.ImpactDetails.Where(s => s.MonthStartDate >= startWeek.FirstDayOfMonth() && s.MonthStartDate <= monthEndDate).ToList();
                        phase.ImpactDetailsByWeeks = phase.ImpactDetails.Where(s => s.WeekStartDate >= startWeek && s.WeekStartDate <= weekEndDate).ToList();
                    }

                    phase.ImpactDetails = new List<PhaseImpactDetailModel>();
                }
            }

            var projectRevenuesHaveValue = projectRevenues.Where(x => x.ProjectValue > 0).ToList();

            return (projectRevenuesHaveValue, consumedTimes);
        }

        private static List<PhaseImpactDetailModel> FilterImpactDetails(PhaseRevenueModel phaseRevenue)
        {
            var StatutoryCode = "LVSTA";
            var impactDetails = new List<PhaseImpactDetailModel>();
            var leaveWeekStartDates = phaseRevenue.RevenueByWeeks.Where(x => x.ImpactLeave == 0).Select(x => x.StartDate).Distinct().ToList();
            var leaveDetails = leaveWeekStartDates.Any() 
                ? phaseRevenue.ImpactDetails.Where(x => x.ImpactCode != StatutoryCode && !leaveWeekStartDates.Contains(x.WeekStartDate.Date)).Distinct().ToList() 
                : phaseRevenue.ImpactDetails.Where(x => x.ImpactCode != StatutoryCode).Distinct().ToList();

            impactDetails.AddRange(leaveDetails);

            var holidayWeekStartDates = phaseRevenue.RevenueByWeeks.Where(x => x.ImpactStatDays == 0).Select(x => x.StartDate).Distinct().ToList();
            var holidayDetails = holidayWeekStartDates.Any()
                ? phaseRevenue.ImpactDetails.Where(x => x.ImpactCode == StatutoryCode && !holidayWeekStartDates.Contains(x.WeekStartDate.Date)).Distinct().ToList()
                : phaseRevenue.ImpactDetails.Where(x => x.ImpactCode == StatutoryCode).Distinct().ToList();
           
            impactDetails.AddRange(holidayDetails);

            return impactDetails.Distinct().ToList();
        }

        private async Task<List<int>> GetProjectIdsByLoginUserAsync()
        {
            var userRole = await _currentUserService.GetUserRoleAsync();

            if (userRole == Constants.UserRole.ProjectManager)
            {
                var projects = await _projectRepository.GetProjectsByUserAsync(_currentUserService.Username);
                var projectIds = projects.Select(x => x.ProjectId).Distinct().ToList();

                return projectIds;
            }
            else
            {
                return new List<int>();
            }
        }

        public async Task<List<string>> GetProjectCodeListAsync()
        {
            var result = await _projectRepository.GetProjectCodeListAsync();
            return result;
        }

        public async Task<bool> IsUserHasAccessToProjectAsync(int id)
        {
            return await _projectRepository.IsUserHasAccessToProjectAsync(id, _currentUserService.Username, await _currentUserService.GetUserRoleAsync());
        }

        private async Task<List<ProjectDetailRateModel>> HandleProcessProjectRates(List<ProjectDetailRateModel> projectDetailRateModel)
        {
            if (projectDetailRateModel != null)
            {
                var projectRateHistories = await _projectRateRepository.GetCurrentRatesAsync(projectDetailRateModel.Select(x => x.ProjectRateId));
                foreach (var item in projectDetailRateModel)
                {
                    var projectRateHistory = projectRateHistories.FirstOrDefault(x => x.ProjectRateId == item.ProjectRateId);
                    if (projectRateHistory != null)
                    {
                        item.HourlyRate = (decimal?)projectRateHistory.Rate;
                        item.EffectiveDate = projectRateHistory.StartDate;
                    }
                }
            }

            return projectDetailRateModel;
        }

        private async Task<bool> CheckExistingLinkedPhase(ProjectDetailAddEditModel project, Project existingProject)
        {
            if (existingProject == null
                || string.IsNullOrEmpty(existingProject.ProjectCode)
                || existingProject.ProjectType == ProjectType.Project)
            {
                return false;
            }

            if (!existingProject.ProjectCode.Equals(project.ProjectCode))
            {
                return await CheckProjectHasLinkedPhase(existingProject.ProjectId);
            }

            return false;
        }

        private async Task UpdateProjectManagers(IList<int> newProjectManagerIds, Project project)
        {
            var employeeManagers = await _projectEmployeeManagerRepository.GetAsync(x => x.ProjectId == project.ProjectId);
            var deletedEmployeeManagers = employeeManagers.Where(x => !newProjectManagerIds.Contains(x.EmployeeId));
            var newEmployeeManagers = new List<ProjectEmployeeManager>();

            foreach (var employeeId in newProjectManagerIds)
            {
                if (!employeeManagers.Any(x => x.EmployeeId == employeeId))
                {
                    newEmployeeManagers.Add(new ProjectEmployeeManager
                    {
                        EmployeeId = employeeId,
                        ProjectId = project.ProjectId,
                    });
                }
            }

            if (newEmployeeManagers.Any())
            {
                await _projectEmployeeManagerRepository.UpdateRange(newEmployeeManagers);
            }

            if (deletedEmployeeManagers.Any())
            {
                await _projectEmployeeManagerRepository.DeleteRangeAsync(deletedEmployeeManagers);
            }

            await _projectEmployeeManagerRepository.SaveChangesAsync();
        }
    }
}
