using AutoMapper;
using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using static ForecastingSystem.Domain.Common.Constants;

namespace ForecastingSystem.DataSyncServices.DataAccessServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IDataSyncServiceLogger _dataSyncLogger;
        private readonly ISyncUserIdLookupRepository _userIdLookupRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IDataSyncServiceLogger dataSyncLogger
            , ISyncUserIdLookupRepository userIdLookupRepository
            , IMapper mapper)
        {
            _dataSyncLogger = dataSyncLogger;
            _userIdLookupRepository = userIdLookupRepository;
            _mapper = mapper;
        }

        public async Task AddOrUpdateRangeAsync(IEnumerable<Outbound.Employee> tsEmployees, CancellationToken cancellationToken)
        {
            try
            {
                await UpdateUserIdLookupsAsync(tsEmployees, cancellationToken);
            }
            catch (CustomExceptions.CustomException ex)
            {
                _dataSyncLogger.LogError($"{GetType().Name} - SyncProcess skipped invalid data", ex);
            }
        }

        private async Task UpdateUserIdLookupsAsync(IEnumerable<Outbound.Employee> tsEmployees, CancellationToken cancellationToken = default)
        {
            Dictionary<string, (string, string)> usernames = new();

            tsEmployees.ToList().ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.UserName))
                {
                    usernames.TryAdd(x.UserName, (x.UserName, x.Email));
                }
            });

            Dictionary<int, UserIdLookup> existingUserIdLookups = new();
            var userIdLookUps = (await _userIdLookupRepository.GetAsync(x => x.TimesheetUserName == null || x.TimesheetEmail == null)).ToList();

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
                        if (string.IsNullOrEmpty(entity.TimesheetUserName) || entity.TimesheetEmail == null)
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
    }
}
