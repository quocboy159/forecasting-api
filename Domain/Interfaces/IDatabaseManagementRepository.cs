using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IDatabaseManagementRepository
    {
        Task DropTables();
        Task RecreateTables();
        Task ResetDataSyncTypeAsync(string dataSyncType);
    }
}
