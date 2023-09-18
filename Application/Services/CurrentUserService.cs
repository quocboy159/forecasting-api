using ForecastingSystem.Application.Common;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Services
{
    public class CurrentUserService: ICurrentUserService
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly ITimesheetService _timesheetService;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, ITimesheetService timesheetService)
        {
            _httpContextAccessor = httpContextAccessor;
            _timesheetService = timesheetService;
        }

        public string? Username
        {
            get
            {
                var userEmail = _httpContextAccessor?.HttpContext?.User.Identity.Name;

                if (!string.IsNullOrEmpty(userEmail))
                {
                    return userEmail.GetUsernameFromEmail();
                };

                return null;
            }
        }

        public async Task<string> GetUserRoleAsync()
        {
            var userType = await _timesheetService.GetUserRoleTypeAsync(Username);
            return userType.Name;
        }

        public async Task<EmployeeRoleTypeModel> GetUserRoleTypeAsync()
        {
            return await _timesheetService.GetUserRoleTypeAsync(Username);
        }

        //private string GetUsernameFromEmail(string userEmail)
        //{
        //    if (string.IsNullOrEmpty(userEmail)) return null;

        //    int index = userEmail.IndexOf("@");
        //    if (index > 0)
        //    {
        //        return userEmail.Substring(0, index);
        //    }

        //    return "";
        //}
    }
}
