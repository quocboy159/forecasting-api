using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface IDataSyncProcessManagementService
    {
        Task<List<DataSyncProcessModel>> GetAll();
    }
}
