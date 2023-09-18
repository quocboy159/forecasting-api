using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface IResourceUtilisationService
    {
        Task<(List<ResourceUtilisationModel>, List<string>)> GetResourceUtilisations(DateTime startWeek, int maxNumberOfWeeks = 52);
        Task<ResourceUtilisationNoteModel> AddOrUpdateResourceUtilisationNote();
        List<ProjectResourceUtilisationModel> GetActualResourceUtilisations(List<EmployeeTimesheetEntry> actualTimesheetEntries, DateTime startWeek, List<UserIdLookup> userIdLookup);
        List<ProjectResourceUtilisationModel> GetProjectResourceUtilisations(List<PhaseResourceView> phaseResourceUtilisations, DateTime startWeek, DateTime limitEndDate);
        
    }
}
