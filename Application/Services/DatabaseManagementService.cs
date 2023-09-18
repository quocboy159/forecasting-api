using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Services
{
    public class DatabaseManagementService : IDatabaseManagementService
    {
        private IDatabaseManagementRepository _databaseManagementRepository;

        public DatabaseManagementService(IDatabaseManagementRepository databaseManagementRepository)
        {
            _databaseManagementRepository = databaseManagementRepository;
        }

        public async Task DropTables()
        {
           await _databaseManagementRepository.DropTables();
        }

        public async Task RecreateTables()
        {
           await _databaseManagementRepository.RecreateTables();
        }

        public async Task ResetDataSyncTypeAsync(string dataSyncType)
        {
           await _databaseManagementRepository.ResetDataSyncTypeAsync(dataSyncType);
        }
    }
}
