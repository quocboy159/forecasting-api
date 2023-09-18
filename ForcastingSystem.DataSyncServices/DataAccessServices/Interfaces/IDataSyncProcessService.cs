using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces
{
    public interface IDataSyncProcessService
    {
        Task<DataSyncProcess> GetByTypeAsync(string dataSyncType, CancellationToken cancellationToken);
        Task<DataSyncProcess> UpdateSyncProcessToInprogressAsync(DataSyncProcess syncInfo, CancellationToken cancellationToken);

        /// <summary>
        /// Update dataSyncType to success or Add new dataSyncType record with success status
        /// </summary>
        /// <param name="dataSyncType"></param>
        /// <param name="lastSyncTime"></param>
        /// <returns></returns>
        Task<DataSyncProcess> UpdateSyncProcessToSuccessAsync(DataSyncProcess syncInfo, DateTime lastSyncTime, CancellationToken cancellationToken);

        /// <summary>
        /// Update dataSyncType to failed or Add new dataSyncType record with failed status
        /// </summary>
        /// <param name="dataSyncType"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        Task<DataSyncProcess> UpdateSyncProcessToFailedAsync(DataSyncProcess syncInfo, string errorMsg, CancellationToken cancellationToken);
    }
}
