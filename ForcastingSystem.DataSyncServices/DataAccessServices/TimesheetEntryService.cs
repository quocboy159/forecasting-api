using AutoMapper;
using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.DataSyncServices.Outbound;
using ForecastingSystem.Domain.Interfaces.SyncInterfaces;
using ForecastingSystem.Domain.Models;
using System.Globalization;

namespace ForecastingSystem.DataSyncServices.DataAccessServices
{
    public class TimesheetEntryService : ITimesheetEntryService
    {
        private readonly ISyncEmployeeTimesheetEntryRepository _timesheetEntryRepository;
        private readonly IDataSyncServiceLogger _dataSyncLogger;
        private readonly IMapper _mapper;
        public TimesheetEntryService(
            ISyncEmployeeTimesheetEntryRepository timesheetEntryRepository
            , IDataSyncServiceLogger dataSyncLogger
            , IMapper mapper)
        {
            _timesheetEntryRepository = timesheetEntryRepository;
            _dataSyncLogger = dataSyncLogger;
            _mapper = mapper;
        }

        public async Task AddOrUpdateRangeAsync(IEnumerable<TimesheetEntry> timesheetEntries, CancellationToken cancellationToken)
        {
            var validEmployeeTSEntries = timesheetEntries.Where(s => !string.IsNullOrEmpty(s.Username) &&
                                                                    s.ProjectId != 0 &&
                                                                    !string.IsNullOrEmpty(s.ProjectName) &&
                                                                    !string.IsNullOrEmpty(s.PhaseCode) &&
                                                                    s.StartDate != DateTime.MinValue &&
                                                                    s.EndDate != DateTime.MinValue &&
                                                                    s.Hours > 0).ToList();
            var employeeTSEntries = new List<EmployeeTimesheetEntry>();
            foreach (var g in validEmployeeTSEntries)
            {
                var employeeTSEntry = new EmployeeTimesheetEntry
                {
                    ExternalProjectId = g.ProjectId,
                    ProjectName = g.ProjectName,
                    PhaseCode = g.PhaseCode,
                    ExternalRateId = g.RateID,
                    RateName = g.RateName,
                    RateAmount= g.RateAmount,
                    TimesheetUsername = g.Username,
                    ExternalTimesheetId = g.TimesheetId,// String.Join(",", g.TSEntries.OrderBy(x => x.TimesheetId).Select(x => x.TimesheetId).ToArray()),
                    Hours = g.Hours,
                    StartDate= g.StartDate, 
                    EndDate= g.EndDate,
                };
                
                employeeTSEntries.Add(employeeTSEntry);
            }

            var allEmployeeTSEntries = await _timesheetEntryRepository.GetAllAsync(cancellationToken);
            var existingEmployeeTSEntries = new List<EmployeeTimesheetEntry>();
            var updatedEmployeeTSEntries = new List<EmployeeTimesheetEntry>();
            employeeTSEntries.ForEach(x =>
            {
                var existingItem = allEmployeeTSEntries.FirstOrDefault(t => x.ExternalTimesheetId == t.ExternalTimesheetId);
                if (existingItem != null)
                {
                    x.EmployeeTimesheetEntryId = existingItem.EmployeeTimesheetEntryId;
                    existingEmployeeTSEntries.Add(existingItem);
                    updatedEmployeeTSEntries.Add(x);
                }
            });
            var existingTimesheetIds = new List<int>();
            if (existingEmployeeTSEntries.Any())
            {
                existingTimesheetIds.AddRange(existingEmployeeTSEntries.Select(x => x.EmployeeTimesheetEntryId).ToList());
            }            

            var addedEmployeeTSEntries = employeeTSEntries.Where(x => !existingTimesheetIds.Contains(x.EmployeeTimesheetEntryId));

            existingEmployeeTSEntries = _mapper.Map<List<EmployeeTimesheetEntry>, List<EmployeeTimesheetEntry>>(updatedEmployeeTSEntries, existingEmployeeTSEntries);
            await _timesheetEntryRepository.UpdateRange(existingEmployeeTSEntries);
            await _timesheetEntryRepository.UpdateRange(addedEmployeeTSEntries);
            await _timesheetEntryRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteRangeAsync(IEnumerable<TimesheetDeletedEntry> timesheetDeletedEntries, CancellationToken cancellationToken)
        {
            var deletedTimesheetIds = timesheetDeletedEntries.Select(s=> s.TimesheetId).Distinct().ToList();
            var entriesToDelete = await _timesheetEntryRepository.GetAsync(s => deletedTimesheetIds.Contains(s.ExternalTimesheetId), cancellationToken);
            if(entriesToDelete.Any())
            {
                await _timesheetEntryRepository.DeleteRangeAsync(entriesToDelete);
                await _timesheetEntryRepository.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
