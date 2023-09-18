using ForecastingSystem.Domain.Common;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
//using ForecastingSystem.Infrastructure.Data.Migrations;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class ProjectRepository : GenericAsyncRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ForecastingSystemDbContext dbContext) : base(dbContext)
        {
            /*
                This is the place where we create the logic for query,
                for saving and calling data for that entity.
             */
        }

        public async Task<Project> GetProjectDetailAsync(int id)
        {
            var result = await DbContext.Projects.AsNoTracking()
                                                 .Include(x => x.Client)
                                                 .Include(x => x.ProjectRates)
                                                 .Include(x => x.Phases)
                                                 .Include(x => x.ProjectEmployeeManagers)
                                                 .FirstOrDefaultAsync(x => x.ProjectId == id
                                                                           && (x.ProjectType == ProjectType.Project
                                                                               || (x.ProjectType == ProjectType.Opportunity
                                                                                   && (x.Phases.Any(p => p.Status == PhaseStatus.Active && !p.TimesheetPhaseId.HasValue)
                                                                                        || x.Phases.All(p => p.Status == PhaseStatus.Inactive)))));

            if (result == null)
            {
                return null;
            }

            result.Phases = result.Phases.Where(x => x.Status == PhaseStatus.Active)
                                         .OrderBy(x => x.IsCompleted)
                                         .ThenBy(x => x.PhaseName)
                                         .ToList();

            result.ProjectRates = result.ProjectRates.Where(x => x.Status == ProjectRateStatus.Active).ToList();

            return result;
        }

        public async Task<bool> IsUserHasAccessToProjectAsync(int id, string username, string role)
        {
            if (role == Constants.UserRole.Admin)
            {
                return true;
            }
            else if (role == Constants.UserRole.ProjectManager)
            {
                var employeeId = await GetEmployeeIdFromUsernameAsync(username);

                var result = await DbContext.Projects.AsNoTracking()
                                     .Include(x => x.ProjectEmployeeManagers)
                                     .FirstOrDefaultAsync(x => x.ProjectId == id);

                if (result != null && result.ProjectEmployeeManagers.Any(x => x.EmployeeId == employeeId))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            var result = await DbContext.Projects.AsNoTracking()
                                     .Include(x => x.Client)
                                     .Include(x => x.Phases.Where(p => p.Status == PhaseStatus.Active))
                                     .Where(x => x.Status == ProjectStatus.Active
                                                 && x.CloseDate == null
                                                 && (x.ProjectType == ProjectType.Project
                                                     || (x.ProjectType == ProjectType.Opportunity
                                                        && (x.Phases.Any(p => p.Status == PhaseStatus.Active && !p.TimesheetPhaseId.HasValue)
                                                            || x.Phases.All(p => p.Status == PhaseStatus.Inactive)))))
                                     .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Project>> GetProjectsByUserAsync(string username)
        {
            var employeeId = await GetEmployeeIdFromUsernameAsync(username);

            var result = await DbContext.Projects.AsNoTracking()
                                     .Include(x => x.Client)
                                     .Include(x => x.Phases.Where(p => p.Status == PhaseStatus.Active))
                                     .Include(x => x.ProjectEmployeeManagers)
                                     .Where(x => x.Status == ProjectStatus.Active
                                                 && x.CloseDate == null
                                                 && x.ProjectEmployeeManagers.Any(p => p.EmployeeId == employeeId)
                                                 && (x.ProjectType == ProjectType.Project
                                                     || (x.ProjectType == ProjectType.Opportunity
                                                         && (x.Phases.Any(p => p.Status == PhaseStatus.Active && !p.TimesheetPhaseId.HasValue)
                                                              || x.Phases.All(p => p.Status == PhaseStatus.Inactive)))))
                                     .ToListAsync();

            return result;
        }

        public async Task<List<Project>> GetProjectsToCalculateRevenue(List<int> projectIds = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            ProjectStatus? status = null,
            bool? excludeLinkedOpportunityToProject = null,
            bool? isClosed = null,
            bool? isCompleted = null
            )
        {
            var query = DbContext.Projects.AsNoTracking()
                                                 .Include(x => x.Client).Include(x => x.ProjectEmployeeManagers).ThenInclude(x => x.Employee)
                                                 .Include(x => x.Phases).ThenInclude(x => x.PhaseResources).ThenInclude(x => x.PhaseResourceExceptions)
                                                 .Include(x => x.Phases).ThenInclude(x => x.PhaseResources).ThenInclude(x => x.Employee)
                                                 .Include(x => x.Phases).ThenInclude(x => x.PhaseResources).ThenInclude(x => x.ResourcePlaceHolder)
                                                 .Include(x => x.Phases).ThenInclude(x => x.PhaseResources).ThenInclude(x => x.ProjectRate).ThenInclude(x => x.ProjectRateHistories)
                                                 .AsQueryable();
            if (projectIds != null && projectIds.Any())
            {
                query = query.Where(x => projectIds.Contains(x.ProjectId));
            }

            if (startDate.HasValue)
            {
                query = query.Where(x => !x.StartDate.HasValue || x.StartDate.Value.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(x => !x.StartDate.HasValue || x.StartDate.Value.Date <= endDate.Value.Date);
            }
            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }
            if (isCompleted.HasValue)
            {
                if (isCompleted.Value == true)
                    query = query.Where(x => x.CompletionDate != null);
                else
                    query = query.Where(x => x.CompletionDate == null);
            }
            if (isClosed.HasValue)
            {
                if (isClosed.Value == true)
                    query = query.Where(x => x.CloseDate != null);
                else
                    query = query.Where(x => x.CloseDate == null);
            }
            if (excludeLinkedOpportunityToProject.HasValue)
            {
                // negative of linked Opportunity search
                query = query.Where(x => !(x.ProjectType == Constants.ProjectType.Opportunity && !string.IsNullOrEmpty(x.ProjectCode)));
            }
            var projects = await query.ToListAsync();
            foreach (var project in projects)
            {
                project.Phases = project.Phases.Where(x => x.Status == PhaseStatus.Active).ToList();
            }
            projects = projects.Where(s => s.Phases.Any()).ToList();
            return projects;
        }

        public async Task<List<string>> GetProjectCodeListAsync()
        {
            var result = await DbContext.Projects.AsNoTracking().Where(x => x.ProjectType == Constants.ProjectType.Project
                                                                            && !string.IsNullOrEmpty(x.ProjectCode)
                                                                            && x.Status == ProjectStatus.Active && x.CloseDate == null).Select(x => x.ProjectCode)
                                                                            .OrderBy(x => x).ToListAsync();
            return result;
        }

        public async Task<bool> IsOpportunityAsync(int projectId)
        {
            var isOpportunity = await DbContext.Projects.AsNoTracking().Where(x => x.ProjectId == projectId && x.ProjectType == Constants.ProjectType.Opportunity).AnyAsync();

            return isOpportunity;
        }

        private async Task<int> GetEmployeeIdFromUsernameAsync(string username)
        {
            var record = await DbContext.UserIdLookups.AsNoTracking().Where(x => x.TimesheetUserName == username).FirstOrDefaultAsync();
            int employeeId = 0;

            if (record != null)
            {
                string bambooUsername = $"{record.BambooHRFirstName.ToLower()}.{record.BambooHRLastName.ToLower()}";
                var employee = await DbContext.Employees.AsNoTracking().Where(x => x.Email == record.BambooHREmail || x.UserName == bambooUsername).FirstOrDefaultAsync();
                if (employee != null) employeeId = employee.EmployeeId;
            }
            else
            {
                var employee = await DbContext.Employees.AsNoTracking().Where(x => x.UserName == username).FirstOrDefaultAsync();
                if (employee != null) employeeId = employee.EmployeeId;
            }

            if (employeeId == 0) throw new Exception("Can not find employee id from username.");

            return employeeId;
        }

        public async Task UpdateProjectValues(int projectId, decimal projectValue, decimal projectBudget)
        {
            string sql = "UPDATE Project " +
                         "SET " +
                "             IsObsoleteProjectValue = 0,    " +
                "            ProjectBudget = @projectBudget,    " +
                "            ProjectValue = @projectValue   " +
                "        WHERE ProjectId = @projectId";

            var projectBudgetParam = new SqlParameter("@projectBudget", projectBudget);
            var projectValueParam = new SqlParameter("@projectValue", projectValue);
            var projectIdParam = new SqlParameter("@projectId", projectId);
            int rowsAffected = await DbContext.Database.ExecuteSqlRawAsync(sql, projectBudgetParam, projectValueParam, projectIdParam);
        }

        public async Task ClearProjectValue(int projectId)
        {
            string sql = "UPDATE Project " +
                             "SET " +
                    "             IsObsoleteProjectValue = 0    " +
            "        WHERE ProjectId = @projectId";

            var projectIdParam = new SqlParameter("@projectId", projectId);
            int rowsAffected = await DbContext.Database.ExecuteSqlRawAsync(sql, projectIdParam);
        }
    }
}
