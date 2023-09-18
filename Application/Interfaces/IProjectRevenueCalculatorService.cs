using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface IProjectRevenueCalculatorService
    {
        ProjectRevenueModel GetProjectRevenue(Project project, List<PublicHoliday> holidays, List<EmployeeLeave> leaves, List<UserIdLookup> userIdsLookup);
        List<ProjectRevenueModel> GetProjectRevenues(List<Project> projects, List<PublicHoliday> holidays, List<EmployeeLeave> leaves, List<UserIdLookup> userIdsLookup, bool mustRecalculate = false);
        PhaseRevenueModel GetPhaseRevenue(Phase phase, List<PublicHoliday> holidays, List<EmployeeLeave> leaves, List<UserIdLookup> userIdsLookup);
        //List<Project> realProjects, List<PublicHoliday> holidays, List<EmployeeLeave> leaves, List<UserIdLookup> userIdLookups, 
        List<ProjectPhaseActualRevenueModel> GetActualProjectRevenues(List<EmployeeTimesheetEntry> timesheetEntries, DateTime startWeek, DateTime endWeek);
    }
}
