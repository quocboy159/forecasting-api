using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.Domain.Common;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.DataSyncServices.DataAccessServices
{
    public class DataSyncProcessService : IDataSyncProcessService
    {
        private IDataSyncProcessRepository _dataSyncProcessRepository;
        private readonly DatetimeHelper _dateTimeHelper;

        public DataSyncProcessService(IDataSyncProcessRepository dataSyncProcessRepository
            , DatetimeHelper datetimeHelper)
        {
            _dataSyncProcessRepository = dataSyncProcessRepository;
            _dateTimeHelper = datetimeHelper;
        }

        public async Task<DataSyncProcess> GetByTypeAsync(string dataSyncType, CancellationToken cancellationToken)
        {
            return await _dataSyncProcessRepository.FirstOrDefaultAsync(x => x.DataSyncType == dataSyncType, cancellationToken);
        }

        public async Task<DataSyncProcess> UpdateSyncProcessToInprogressAsync(DataSyncProcess syncInfo, CancellationToken cancellationToken)
        {
            syncInfo.Status = DataSyncProcessStatuses.Inprogess;
            syncInfo.ErrorMessage = null;
            await _dataSyncProcessRepository.UpdateAsync(syncInfo);
            await _dataSyncProcessRepository.SaveChangesAsync(cancellationToken);
            return syncInfo;
        }

        public async Task<DataSyncProcess> UpdateSyncProcessToSuccessAsync(DataSyncProcess syncInfo, DateTime lastSyncTime, CancellationToken cancellationToken)
        {
            syncInfo.Status = DataSyncProcessStatuses.Success;
            syncInfo.LastSyncDateTime = lastSyncTime;
            syncInfo.FinishDateTime = _dateTimeHelper.Now(); ;
            syncInfo.ErrorMessage = null;
            await _dataSyncProcessRepository.UpdateAsync(syncInfo);
            await _dataSyncProcessRepository.SaveChangesAsync(cancellationToken);
            return syncInfo;
        }

        public async Task<DataSyncProcess> UpdateSyncProcessToFailedAsync(DataSyncProcess syncInfo, string errorMsg, CancellationToken cancellationToken)
        {
            syncInfo.Status = DataSyncProcessStatuses.Failed;
            syncInfo.FinishDateTime = _dateTimeHelper.Now();
            syncInfo.ErrorMessage = errorMsg;
            await _dataSyncProcessRepository.UpdateAsync(syncInfo);
            await _dataSyncProcessRepository.SaveChangesAsync(cancellationToken);
            return syncInfo;
        }
    }

}
