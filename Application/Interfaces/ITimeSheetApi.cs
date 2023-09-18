using ForecastingSystem.Application.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface ITimeSheetApi
    {
        [Get("/api/Forecast/GetUserPermission")]
        Task<EmployeeRoleModel> GetUserPermissionAsync(string username, [Header("X-ApiKey")] string apiKey);
        [Post("/api/Forecast/UpdatePhaseBudget")]
        Task UpdatePhaseBudget([Body] UpdatePhaseBudgetModel updatePhaseBudgetModel, [Header("X-ApiKey")] string apiKey);
    }
}
