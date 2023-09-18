using AutoMapper;
using ForecastingSystem.DataSyncServices.DataAccessServices.CustomExceptions;
using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using System.Threading;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.DataSyncServices.DataAccessServices
{
    public class ProjectService : IProjectService
    {
        private readonly ISyncProjectRepository _projectRepository;
        private readonly ISyncEmployeeRepository _employeeRepository;
        private readonly ISyncClientRepository _clientRepository;
        private readonly IDataSyncServiceLogger _dataSyncLogger;
        private readonly IMapper _mapper;
        private readonly ISyncUserIdLookupRepository _userIdLookupRepository;
        private readonly ISyncProjectEmployeeManagerRepository _employeeManagerRepository;

        public ProjectService(
            ISyncProjectRepository projectRepository
            , ISyncEmployeeRepository employeeRepository
            , ISyncClientRepository clientRepository
            , IDataSyncServiceLogger dataSyncLogger
            , IMapper mapper
            , ISyncUserIdLookupRepository userIdLookupRepository
            , ISyncProjectEmployeeManagerRepository syncProjectEmployeeManagerRepository)
        {
            _projectRepository = projectRepository;
            _employeeRepository = employeeRepository;
            _clientRepository = clientRepository;
            _dataSyncLogger = dataSyncLogger;
            _mapper = mapper;
            _userIdLookupRepository = userIdLookupRepository;
            _employeeManagerRepository = syncProjectEmployeeManagerRepository;
        }
        public async Task AddOrUpdateRangeAsync(IEnumerable<Outbound.Project> tsProjects, CancellationToken cancellationToken)
        {
            try
            {
                await UpdateUserIdLookupsAsync(tsProjects, cancellationToken);
                var userNames = GetProjectManagerUserNames(tsProjects);
                var userIdlookups = await _userIdLookupRepository.GetAsync(x => userNames.Contains(x.TimesheetUserName), cancellationToken);
                var employees = await _employeeRepository.GetAsync(x => userNames.Contains(x.UserName), cancellationToken);
                var externalClientIds = tsProjects.Select(x => x.OrganizationId).ToList();
                var clients = await _clientRepository.GetAsync(x => externalClientIds.Contains(x.ExternalClientId ?? 0));
                var missingClientProjects = new List<Outbound.Project>();
                var timeSheetProjectManagers = new Dictionary<string, IList<int>>();

                tsProjects.ToList().ForEach(x =>
                {
                    var client = clients.FirstOrDefault(y => y.ExternalClientId == x.OrganizationId);
                    if (client == null)
                    {
                        missingClientProjects.Add(x);
                        return;
                    }
                    x.FSClientId = client.ClientId;

                    //var userIdLookup = userIdlookups.FirstOrDefault(y => y.TimesheetUserName == x.ProjectManagerUserName);
                    //if (userIdLookup != null)
                    //{
                    //    //var username = $"{userIdLookup.BambooHRFirstName.ToLower()}.{userIdLookup.BambooHRLastName.ToLower()}";
                    //    var employee = employees.FirstOrDefault(y => y.Email.ToLower() == userIdLookup.BambooHREmail.ToLower());
                    //    if (employee != null)
                    //        x.FSProjectManagerId = employee.EmployeeId;
                    //}

                    var projectManagerIds = new List<int>();
                    if (x.ProjectManagers.Any())
                    {
                        var filteredUserIdLookups = x.ProjectManagers.Any() ? userIdlookups.Where(y => x.ProjectManagers.Any(f => f.UserName.Contains(y.TimesheetUserName))) : new List<UserIdLookup>();
                        var employeeIds = filteredUserIdLookups.Any() ? employees.Where(c => filteredUserIdLookups.Any(p => p.BambooHREmail.Equals(c.Email, StringComparison.OrdinalIgnoreCase))).Select(c => c.EmployeeId) : new List<int>();
                        if (employeeIds.Any())
                        {
                            projectManagerIds.AddRange(employeeIds);
                        }
                    }

                    timeSheetProjectManagers.Add(x.Code, projectManagerIds);

                    //else
                    //{
                    //    var employee = employees.FirstOrDefault(y => y.UserName == x.ProjectManagerUserName);
                    //    if (employee != null)
                    //        x.FSProjectManagerId = employee.EmployeeId;
                    //}
                });

                tsProjects = tsProjects.Where(x => x.FSClientId > 0);
                var allProjects = await _projectRepository.GetAllAsync(cancellationToken);

                var existingProjects = new List<Project>();
                var addedProjects = new List<Project>();
                tsProjects.ToList().ForEach(x =>
                {
                    if (x.DueDate == new DateTime(1900, 01, 01)) x.DueDate = null;
                    if (x.CloseDate == new DateTime(1900, 01, 01)) x.CloseDate = null;
                    if (x.CompletionDate == new DateTime(1900, 01, 01)) x.CompletionDate = null;

                    var existingItem = allProjects.FirstOrDefault(t => t.ExternalProjectId == x.ProjectId);
                    if (existingItem == null)
                    {
                        addedProjects.Add(_mapper.Map<Project>(x));
                    }
                    else
                    {
                        Project updatedItem = _mapper.Map<Project>(x);
                        updatedItem.ProjectId = existingItem.ProjectId;
                        existingProjects.Add(updatedItem);
                    }
                });

                await _projectRepository.UpdateRange(existingProjects);
                await _projectRepository.UpdateRange(addedProjects);
                await _projectRepository.SaveChangesAsync(cancellationToken);

                var updatedProjects = existingProjects.Union(addedProjects).ToList();

                await AddOrUpdateProjectManagers(timeSheetProjectManagers, updatedProjects);

                if (missingClientProjects.Any())
                {
                    NotifyMissingClients(missingClientProjects);
                }
            }
            catch (CustomException ex)
            {
                _dataSyncLogger.LogWarning($"{GetType().Name} - SyncProcess skipped invalid data: {ex.Message}");
            }
        }

        private void NotifyMissingClients(IEnumerable<Outbound.Project> missingClientProjects)
        {
            var missingItems = new Dictionary<int, string>();
            missingClientProjects.ToList().ForEach(
                x =>
                {
                    missingItems.Add(x.ProjectId, $"{x.OrganizationId}/{x.OrganizationName}");
                });
            var msg = String.Format("{0}: {1}", "Projects without existing clients", missingItems.ToJsonString());
            throw new CustomExceptions.InvalidDataException(msg);
        }

        private async Task UpdateUserIdLookupsAsync(IEnumerable<Outbound.Project> tsProjects, CancellationToken cancellationToken = default)
        {
            Dictionary<string, (string, string)> usernames = new();

            tsProjects.ToList().ForEach(x =>
            {
                if (x.ProjectManagers.Any())
                {
                    foreach (var manager in x.ProjectManagers)
                    {
                        usernames.TryAdd(manager.UserName, (manager.UserName, manager.Email));
                    }
                }
            });

            Dictionary<int, UserIdLookup> existingUserIdLookups = new();
            var userIdLookUps = (await _userIdLookupRepository.GetAsync(x => x.TimesheetUserName == null)).ToList();

            foreach (var item in usernames)
            {
                var username = item.Value.Item1;
                var email = item.Value.Item2;

                var parts = username.Split(".");
                if (parts.Length == 2)
                {
                    var timesheetFirstName = parts[0].ToLower();
                    var timesheetLastName = parts[1].ToLower();
                    var entity = userIdLookUps.FirstOrDefault(x => (Utility.GetUsernameFromEmail(x.BambooHREmail).ToLower() == username.ToLower())
                                                                   || (x.BambooHRFirstName.ToLower() == timesheetFirstName && x.BambooHRLastName.ToLower() == timesheetLastName));

                    if (entity != null)
                    {
                        if (string.IsNullOrEmpty(entity.TimesheetUserName))
                        {
                            entity.TimesheetUserName = username.ToLower();
                            entity.TimesheetEmail = email?.ToLower();
                            entity.LastUpdatedBy = DataSyncSources.TimesheetSource;
                            entity.LastUpdatedDateTime = DateTime.UtcNow;
                        }
                        existingUserIdLookups.TryAdd(entity.Id, entity);
                    }
                }
            }

            await _userIdLookupRepository.UpdateRange(existingUserIdLookups.Select(x => x.Value));
            await _userIdLookupRepository.SaveChangesAsync(cancellationToken);
        }

        private IList<string> GetProjectManagerUserNames(IEnumerable<Outbound.Project> tsProjects)
        {
            var userNames = new List<string>();
            var projectManagerUserNames = tsProjects.Where(x => x.ProjectManagers.Any(p => !string.IsNullOrEmpty(p.UserName))).Select(x => x.ProjectManagers.Select(x => x.UserName));
            foreach (var item in projectManagerUserNames)
            {
                userNames.AddRange(item);
            }

            return userNames.Distinct().ToList();
        }

        private async Task AddOrUpdateProjectManagers(IDictionary<string, IList<int>> timeSheetProjectManagers, IList<Project> updatedProjects)
        {
            if (!timeSheetProjectManagers.Any() || !updatedProjects.Any())
            {
                return;
            }

            var projectIds = updatedProjects.Select(x => x.ProjectId);
            var existingProjectEmployeeManagers = await _employeeManagerRepository.GetAsync(x => projectIds.Contains(x.ProjectId));
            var addedProjectManagers = new List<ProjectEmployeeManager>();
            var deletedProjectEmployeeManagers = new List<ProjectEmployeeManager>();

            foreach (var project in updatedProjects)
            {
                timeSheetProjectManagers.TryGetValue(project.ProjectCode, out var projectManagerIds);

                var currentProjectEmployeeManagers = existingProjectEmployeeManagers.Where(x => x.ProjectId == project.ProjectId);
                foreach (var employeeId in projectManagerIds!)
                {
                    if (!currentProjectEmployeeManagers.Any(x => x.EmployeeId == employeeId))
                    {
                        addedProjectManagers.Add(new ProjectEmployeeManager { EmployeeId = employeeId, ProjectId = project.ProjectId });
                    }
                }

                deletedProjectEmployeeManagers.AddRange(currentProjectEmployeeManagers.Where(x => !projectManagerIds.Contains(x.EmployeeId)));
            }

            if (deletedProjectEmployeeManagers.Any())
            {
                await _employeeManagerRepository.DeleteRangeAsync(deletedProjectEmployeeManagers);
            }

            if (addedProjectManagers.Any())
            {
                await _employeeManagerRepository.UpdateRange(addedProjectManagers);
            }

            if (deletedProjectEmployeeManagers.Any() || addedProjectManagers.Any())
            {
                await _employeeManagerRepository.SaveChangesAsync();
            }
        }

        public async Task<List<Project>> GetAll()
        {
            var allProjects = await _projectRepository.GetAllAsync(CancellationToken.None);
            return allProjects.ToList();
        }

        public async Task<List<int>> GetSyncedProjectExternalIds()
        {
            List<Domain.Models.Project> projects = await GetAll();
            List<int> externalProjectIds = projects.Where(x => x.ExternalProjectId != null)
                                                    .Select(x => x.ExternalProjectId.Value).ToList();
            return externalProjectIds;
        }
    }
}
