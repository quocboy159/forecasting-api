using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.DataSyncServices.Interfaces
{
    public interface IDataSyncServiceLogger
    {
        void LogInfo(string message);
        void LogDebug(string message);
        void LogError(string message, Exception ex);
        void EndReport();
        void StartNewReport();
        void LogInfo(string message, object data);
        void LogWarning(string message);
    }
}
