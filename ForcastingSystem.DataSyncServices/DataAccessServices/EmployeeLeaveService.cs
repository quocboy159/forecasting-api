using AutoMapper;
using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using System.Threading;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.DataSyncServices.DataAccessServices
{
    public class EmployeeLeaveService : IEmployeeLeaveService
    {
        private readonly ISyncEmployeeLeaveRepository _employeeLeaveRepository;
        private readonly IDataSyncServiceLogger _dataSyncLogger;
        private readonly IMapper _mapper;
        private readonly ISyncUserIdLookupRepository _userIdLookupRepository;

        public EmployeeLeaveService(ISyncEmployeeLeaveRepository employeeLeaveRepository
            , IDataSyncServiceLogger dataSyncLogger
            , IMapper mapper
            , ISyncUserIdLookupRepository userIdLookupRepository)
        {
            _employeeLeaveRepository = employeeLeaveRepository;
            _dataSyncLogger = dataSyncLogger;
            _mapper = mapper;
            _userIdLookupRepository = userIdLookupRepository;
        }

        public async Task AddOrUpdateRangeAsync(IEnumerable<Outbound.EmployeeLeave> tsEmployeeLeaves, CancellationToken cancellationToken)
        {
            try
            {
                await UpdateUserIdLookupsAsync(tsEmployeeLeaves, cancellationToken);

                var allEmployeeLeaves = await _employeeLeaveRepository.GetAllAsync(cancellationToken);

                var existingEmployeeLeaves = new List<EmployeeLeave>();
                var addedEmployeeLeaves = new List<EmployeeLeave>();
                tsEmployeeLeaves.ToList().ForEach(x =>
                {
                    var existingItem = allEmployeeLeaves.FirstOrDefault(t => t.ExternalLeaveId == x.LeaveId);
                    if (existingItem == null)
                    {
                        addedEmployeeLeaves.Add(_mapper.Map<EmployeeLeave>(x));
                    }
                    else
                    {
                        // Cannot use Automapper here :(
                        existingItem.ExternalLeaveId = x.LeaveId;
                        existingItem.TimesheetUsername= x.UserName;
                        existingItem.Status = x.Status;
                        existingItem.StartDate = x.StartDate;
                        existingItem.EndDate = x.EndDate.HasValue? x.EndDate.Value: DateTime.MinValue;
                        existingItem.DayType = x.DayType;
                        existingItem.SubmissionDate = x.CreatedDate;
                        existingItem.EndDate = DateTime.Now;
                        existingItem.LeaveCode = x.LeaveCode;
                        existingItem.TotalDays = x.TotalDays;
                        existingEmployeeLeaves.Add(existingItem);
                    }
                });

                await _employeeLeaveRepository.UpdateRange(existingEmployeeLeaves);
                await _employeeLeaveRepository.UpdateRange(addedEmployeeLeaves);
                await _employeeLeaveRepository.SaveChangesAsync(cancellationToken);



            }
            catch (CustomExceptions.CustomException ex)
            {
                _dataSyncLogger.LogError($"{GetType().Name} - SyncProcess skipped invalid data", ex);
            }
        }

        private async Task UpdateUserIdLookupsAsync(IEnumerable<Outbound.EmployeeLeave> tsEmployeeLeaves, CancellationToken cancellationToken = default)
        {
            Dictionary<string, string> usernames = new();

            tsEmployeeLeaves.ToList().ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.UserName))
                {
                    usernames.TryAdd(x.UserName, x.UserName);
                }
            });

            Dictionary<int, UserIdLookup> existingUserIdLookups = new();
            var userIdLookUps = (await _userIdLookupRepository.GetAsync(x => x.TimesheetUserName == null)).ToList();

            foreach (var username in usernames)
            {
                var parts = username.Value.Split(".");
                if (parts.Length == 2)
                {
                    var timesheetFirstName = parts[0].ToLower();
                    var timesheetLastName = parts[1].ToLower();
                    var entity = userIdLookUps.FirstOrDefault(x => (Utility.GetUsernameFromEmail(x.BambooHREmail).ToLower() == username.Value.ToLower())
                                                                   || (x.BambooHRFirstName.ToLower() == timesheetFirstName && x.BambooHRLastName.ToLower() == timesheetLastName));

                    if (entity != null)
                    {
                        if (string.IsNullOrEmpty(entity.TimesheetUserName))
                        {
                            entity.TimesheetUserName = username.Value.ToLower();
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
    }
}
