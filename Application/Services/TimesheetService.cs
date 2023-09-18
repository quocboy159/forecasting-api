using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly IMemoryCache _cache;
        private readonly ITimeSheetApi _timeSheetApi;
        private string _apiKey;

        public TimesheetService(IMemoryCache cache, IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue<string>("TimesheetAPI:URL");
            _apiKey = configuration.GetValue<string>("TimesheetAPI:APIKey");

            _timeSheetApi = RestService.For<ITimeSheetApi>(baseUrl);
            _cache = cache;


        }

        public async Task UpdatePhaseBudgetAsync(UpdatePhaseBudgetModel updatePhaseBudgetModel)
        {
            await _timeSheetApi.UpdatePhaseBudget(updatePhaseBudgetModel, _apiKey);
        }

        public async Task<EmployeeRoleTypeModel> GetUserRoleTypeAsync(string username)
        {
            var cacheKey = $"LoggedUserRole:{username}";
            if (_cache.TryGetValue(cacheKey, out EmployeeRoleTypeModel userType))
            {
                return userType;
            }

            EmployeeRoleModel permission;
            try
            {
                permission = await _timeSheetApi.GetUserPermissionAsync(username, _apiKey);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to call timesheet api to get user role.", ex);
            }

            _cache.Set(cacheKey, permission.UserType, TimeSpan.FromMinutes(1));

            return permission.UserType;
        }
    }
}
