using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface IDatabaseManagementService
    {
        Task DropTables();
        Task RecreateTables();
        Task ResetDataSyncTypeAsync(string dataSyncType);
    }
}
