using AutoMapper;
using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.DataSyncServices.DataAccessServices
{
    public class PublicHolidayService : IPublicHolidayService
    {
        private readonly ISyncPublicHolidayRepository _publicHolidayRepository;
        private readonly IDataSyncServiceLogger _dataSyncLogger;
        private readonly IMapper _mapper;

        public PublicHolidayService(ISyncPublicHolidayRepository publicHolidayRepository
            , IDataSyncServiceLogger dataSyncLogger
            , IMapper mapper)
        {
            _publicHolidayRepository = publicHolidayRepository;
            _dataSyncLogger = dataSyncLogger;
            _mapper = mapper;
        }

        public async Task AddOrUpdateRangeAsync(IEnumerable<Outbound.PublicHoliday> tsPublicHolidays, CancellationToken cancellationToken)
        {
            try
            {
                var allPublicHolidays = await _publicHolidayRepository.GetAllAsync(cancellationToken);

                var existingPublicHolidays = new List<PublicHoliday>();
                var addedPublicHolidays = new List<PublicHoliday>();
                var deletedPublicHolidays = new List<PublicHoliday>();

                tsPublicHolidays.Where(x => x.IsActive && x.LeaveHolidayId > 0).ToList().ForEach(x =>
                {
                    var existingItem = allPublicHolidays.FirstOrDefault(t => t.ExternalLeaveHolidayId == x.LeaveHolidayId);
                    if (existingItem == null)
                    {
                        var entity = new PublicHoliday()
                        {
                            Country = x.Country,
                            Date = x.Date,
                            ExternalLeaveHolidayId = x.LeaveHolidayId,
                            Name = x.Name,
                        };

                        addedPublicHolidays.Add(entity);
                    }
                    else
                    {
                        existingItem.Country = x.Country;
                        existingItem.Date = x.Date;
                        existingItem.ExternalLeaveHolidayId = x.LeaveHolidayId;
                        existingItem.Name = x.Name;

                        existingPublicHolidays.Add(existingItem);
                    }
                });

                tsPublicHolidays.Where(x => !x.IsActive && x.LeaveHolidayId > 0).ToList().ForEach(x =>
                {
                    var existingItem = allPublicHolidays.FirstOrDefault(t => t.ExternalLeaveHolidayId == x.LeaveHolidayId);
                    if (existingItem != null)
                    {
                        deletedPublicHolidays.Add(existingItem);
                    }
                });

                await _publicHolidayRepository.UpdateRange(existingPublicHolidays);
                await _publicHolidayRepository.UpdateRange(addedPublicHolidays);
                await _publicHolidayRepository.DeleteRangeAsync(deletedPublicHolidays);
                await _publicHolidayRepository.SaveChangesAsync(cancellationToken);
            }
            catch (CustomExceptions.CustomException ex)
            {
                _dataSyncLogger.LogError($"{GetType().Name} - SyncProcess skipped invalid data", ex);
            }
        }
    }
}
