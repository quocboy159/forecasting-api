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
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Services
{
    public class ResourceUtilisationService : IResourceUtilisationService
    {
        private readonly IEmployeeTimesheetEntryRepository _employeeTimesheetEntryRepository;
        private readonly IUserIdLookupRepository _userIdLookupRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPhaseResourceRepository _phaseResourceRepository;
        private readonly IEmployeeUtilisationNotesRepository _employeeUtilisationNotesRepository;      
        private readonly ICurrentUserService _currentUserService;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        public ResourceUtilisationService(
            IMapper mapper,
            IEmployeeTimesheetEntryRepository employeeTimesheetEntryRepository,
            IPhaseResourceRepository phaseResourceRepository,
            IUserIdLookupRepository userIdLookupRepository,
            IEmployeeRepository employeeRepository,
            IEmployeeUtilisationNotesRepository employeeUtilisationNotesRepository,
            IProjectRepository projectRepository,
            ICurrentUserService currentUserService
            )
        {
            _employeeTimesheetEntryRepository = employeeTimesheetEntryRepository;
            _userIdLookupRepository = userIdLookupRepository;
            _employeeRepository = employeeRepository;
            _phaseResourceRepository = phaseResourceRepository;
            _employeeUtilisationNotesRepository = employeeUtilisationNotesRepository;
            _mapper = mapper;
            _projectRepository = projectRepository;
            _currentUserService = currentUserService;
        }
        public Task<ResourceUtilisationNoteModel> AddOrUpdateResourceUtilisationNote()
        {
            throw new NotImplementedException();
        }
        public async Task<(List<ResourceUtilisationModel>, List<string>)> GetResourceUtilisations(DateTime startWeek, int maxNumberOfWeeks = 52)
        {
            string timeFormat = @"m\:ss\.fff";
            List<string> consumedTimes = new();
            var timer = new Stopwatch();
            timer.Start();

            // get actual timesheet from past until last week
            var startDate = startWeek.CurrentWeekMonday();            
            var limitEndDate = startDate.AddDays(7 * maxNumberOfWeeks).CurrentWeekMonday();
            var currentEndDate = DateTime.Now.Date.CurrentWeekMonday().AddMinutes(-1);
            var endDateTime = limitEndDate > currentEndDate ? currentEndDate : limitEndDate;
          
            var actualTimesheetEntries = _employeeTimesheetEntryRepository.GetEntries(startDate, endDateTime).Result.ToList();
            
            // only get phase resource utilisation from current week or future week ( means past week it's 0 by default)
            var currentOrFutureWeek = new DateTime(Math.Max(startDate.Ticks, DateTime.Now.Date.CurrentWeekMonday().Ticks));
            var projectIds = GetProjectIdsByLoginUserAsync().Result.ToList();
            var phaseResourceUtilisations = _phaseResourceRepository.GetPhaseResourceUtilisations(currentOrFutureWeek).Result.ToList();          

            var userIdLookup = _userIdLookupRepository.GetAllAsync().Result.ToList();
            
            var employees = _employeeRepository.GetEmployeesForResourceUtilisationAsync().Result.ToList();
            var employeeNotes = _employeeUtilisationNotesRepository.GetAllAsync().Result.ToList();
            var employeeNotesDictionary = employeeNotes.ToDictionary(s => s.EmployeeId);
            timer.Stop();
            consumedTimes.Add($"Resource Utilisation - Query data prepare to calculate: {timer.Elapsed.ToString(timeFormat)}");
            timer.Start();
            var actual = GetActualResourceUtilisations(actualTimesheetEntries, startDate, userIdLookup);
            var actualHoursDictionary = actual.GroupBy(s => s.UserName.ToLower()).ToDictionary(s => s.Key, x => x.ToList());
            var projectResourceUtilisations = GetProjectResourceUtilisations(phaseResourceUtilisations, currentOrFutureWeek, limitEndDate).ToList();            
            var projectRealEmployeeUtilisations = projectResourceUtilisations.Where(s => !s.IsPlaceHolder).ToList();
            var projectRealEmployeeUtilisationsDictionary = projectRealEmployeeUtilisations.GroupBy(s => s.UserName.ToLower()).ToDictionary(s => s.Key, x => x.ToList());
            timer.Stop();
            consumedTimes.Add($"Resource Utilisation - Calculate the resource utilisation: {timer.Elapsed.ToString(timeFormat)}");
            timer.Start();
            var result = new List<ResourceUtilisationModel>();
            foreach ( var employee in employees)
            {
                var resource = new ResourceUtilisationModel() {
                    UserName= employee.UserName,
                    FullName= employee.FullName,
                    EmployeeId= employee.EmployeeId,
                    WorkingHours = employee.WorkingHoursNumber,
                };
                var employeeUserNameKey = employee.UserName.ToLower();
                if (actualHoursDictionary.ContainsKey(employeeUserNameKey))
                {
                    resource.ProjectResourceUtilisations = actualHoursDictionary[employeeUserNameKey];
                }
                if (projectRealEmployeeUtilisationsDictionary.ContainsKey(employeeUserNameKey))
                {
                    foreach(var project in projectRealEmployeeUtilisationsDictionary[employeeUserNameKey])
                    {
                        var existingProject = resource.ProjectResourceUtilisations.FirstOrDefault(s => s.ExternalProjectId.HasValue && s.ExternalProjectId == project.ExternalProjectId);
                        if (existingProject != null)
                        {
                            if (existingProject.LargestWeek.HasValue) // add continuously week from phase resource week list
                                project.UtilisationByWeeks = project.UtilisationByWeeks.Where(s => s.StartDate > existingProject.LargestWeek.Value).ToList();
                            existingProject.UtilisationByWeeks.AddRange(project.UtilisationByWeeks);
                        }
                        else
                        {
                            resource.ProjectResourceUtilisations.Add(project);
                        }
                    }
                }
                resource.ProjectResourceUtilisations.ForEach(p =>
                {
                    p.UtilisationByWeeks.ForEach(w => w.SetWorkingHours(resource.WorkingHours));
                });

                if (employeeNotesDictionary.ContainsKey(employee.EmployeeId))
                {
                    resource.Notes = _mapper.Map<EmployeeUtilisationNotesModel>(employeeNotesDictionary[employee.EmployeeId]);
                }
                result.Add(resource);
            }

            // handle more Placeholder resource
            var projectPlaceHolderUtilisations = projectResourceUtilisations.Where(s => s.IsPlaceHolder).ToList();
            foreach (var placeholder in projectPlaceHolderUtilisations)
            {
                placeholder.FullName = placeholder.UserName;
                var resource = new ResourceUtilisationModel()
                {
                    UserName = placeholder.UserName,
                    ResourcePlaceHolderId = placeholder.ResourcePlaceHolderId.Value,
                    WorkingHours = 40, // by default
                    ProjectResourceUtilisations = new List<ProjectResourceUtilisationModel> { placeholder }
                };
                resource.ProjectResourceUtilisations.ForEach(p =>
                {
                    p.UtilisationByWeeks.ForEach(w => w.SetWorkingHours(resource.WorkingHours));
                });
                result.Add(resource);
            }
            timer.Stop();
            consumedTimes.Add($"Resource Utilisation - Binding the resource utilisation: {timer.Elapsed.ToString(timeFormat)}");
            
            result = LimitRecords(result, startDate, maxNumberOfWeeks);
            return (result, consumedTimes);
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

        private string GetEmployeeUsername(string timesheetUsername, Dictionary<string, UserIdLookup> userIdLookupDictionary)
        {
            if (userIdLookupDictionary.ContainsKey(timesheetUsername))
            {
                var username = userIdLookupDictionary[timesheetUsername].BambooHREmail.GetUsernameFromEmail();
                if (!string.IsNullOrEmpty(username)) { return username; }
            }
            return timesheetUsername;
        }
        public List<ProjectResourceUtilisationModel> GetActualResourceUtilisations(List<EmployeeTimesheetEntry> actualTimesheetEntries, DateTime startWeek, List<UserIdLookup> userIdLookup)
        {
            var projects = _projectRepository.GetAllProjectsAsync().Result.ToList();

            var userIdLookupDictionary = userIdLookup.Where(s => !string.IsNullOrEmpty(s.TimesheetUserName)).ToDictionary(s => s.TimesheetUserName.ToLower());
            var result = new List<ProjectResourceUtilisationModel>();
            var employeeProjects = actualTimesheetEntries.GroupBy(s => new { s.TimesheetUsername, s.ExternalProjectId, s.ProjectName }).ToList();
            foreach (var employeeProject in employeeProjects)
            {                
                var model = new ProjectResourceUtilisationModel()
                {
                    ExternalProjectId = employeeProject.Key.ExternalProjectId, 
                    ProjectName = employeeProject.Key.ProjectName,                   
                    UserName = GetEmployeeUsername(employeeProject.Key.TimesheetUsername.ToLower(), userIdLookupDictionary),
                    UtilisationByWeeks = new List<ResourceUtilisationDetailModel>()
                };

                var project = projects.FirstOrDefault(x => x.ExternalProjectId == employeeProject.Key.ExternalProjectId);
                if (project != null)
                {
                    model.ProjectId = project.ProjectId;
                    model.ProjectCode = project.ProjectCode;
                    model.ProjectType = project.ProjectType;
                    model.ClientName = project.Client.ClientName;
                }
                var employeeEntries = employeeProject.ToList();
                var startDate = startWeek;
                while (employeeEntries.Any())
                {
                    var endDate = startDate.NextWeekMonday().AddMinutes(-1);
                    var entries = employeeEntries.Where(s => s.StartDate >= startDate && s.EndDate <= endDate).ToList();
                    if (entries.Any())
                    {
                        model.UtilisationByWeeks.Add(new ResourceUtilisationDetailModel()
                        {
                            StartDate = startDate,
                            Hours = (float)entries.Sum(s => s.Hours),
                            IsActual = true
                        });
                    }
                    startDate = startDate.NextWeekMonday();
                    employeeEntries = employeeEntries.Except(entries).ToList();
                }
                result.Add(model);
            }

            return result;
        }

        public List<ProjectResourceUtilisationModel> GetProjectResourceUtilisations(List<PhaseResourceView> phaseResourcesView, DateTime startWeek, DateTime limitEndDate)
        {   
            var result = new List<ProjectResourceUtilisationModel>();
            var projectIds = GetProjectIdsByLoginUserAsync().Result;
            var employeeProjects = phaseResourcesView.GroupBy(s => new { s.EmployeeId, s.Username, s.ResourcePlaceHolderId, s.ResourcePlaceHolderName, s.ProjectName, s.ProjectCode, s.ProjectType, s.ClientName, s.ProjectId, s.ExternalProjectId }).ToList();
            
            foreach (var employeeProject in employeeProjects)
            {
                var model = new ProjectResourceUtilisationModel()
                {
                    ProjectId = employeeProject.Key.ProjectId,
                    ProjectName = employeeProject.Key.ProjectName,
                    ProjectCode = employeeProject.Key.ProjectCode, 
                    ProjectType = employeeProject.Key.ProjectType, 
                    ClientName = employeeProject.Key.ClientName,
                    IsPMProject = projectIds.Contains(employeeProject.Key.ProjectId),
                    UserName = employeeProject.Key.EmployeeId.HasValue ? employeeProject.Key.Username : employeeProject.Key.ResourcePlaceHolderName,
                    IsPlaceHolder = employeeProject.Key.ResourcePlaceHolderId.HasValue,
                    ResourcePlaceHolderId = employeeProject.Key.ResourcePlaceHolderId,
                    ExternalProjectId = employeeProject.Key.ExternalProjectId,
                };
                if (model.IsPlaceHolder)
                {
                    model.ResourcePlaceHolder_FTE = (float)employeeProject.ElementAt(0).FTE;
                    model.ResourcePlaceHolder_PhaseName = employeeProject.ElementAt(0).PhaseName;
                }
                
                var utilisationByWeeks = employeeProject.SelectMany(s => s.PhaseResourceUtilisations)
                    .Where(s=>s.StartWeek >= startWeek && s.StartWeek < limitEndDate).ToList();
                var startDate = startWeek;
                while (utilisationByWeeks.Any())
                {
                    var endDate = startDate.NextWeekMonday().AddMinutes(-1);
                    
                    var entries = utilisationByWeeks.Where(s => s.StartWeek == startDate).ToList();
                    if (entries.Any())
                    {
                        model.UtilisationByWeeks.Add(new ResourceUtilisationDetailModel()
                        {
                            StartDate = startDate,
                            Hours = (float)entries.Sum(s => s.TotalHours),
                        });
                    }
                    startDate = startDate.NextWeekMonday();
                    utilisationByWeeks = utilisationByWeeks.Except(entries).ToList();
                }
                result.Add(model);
            }

            return result;
        }

        private List<ResourceUtilisationModel> LimitRecords(List<ResourceUtilisationModel> resources, DateTime startWeek, int numberOfWeeks)
        {
            foreach (var resourceUtilisation in resources)
            {
                foreach (var project in resourceUtilisation.ProjectResourceUtilisations)
                {
                    project.UtilisationByWeeks = project.UtilisationByWeeks.Where(s => s.StartDate >= startWeek)
                        .Take(numberOfWeeks).ToList();
                }
            }
            return resources;
        }
    }
}
