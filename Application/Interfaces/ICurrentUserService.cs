using ForecastingSystem.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string? Username { get; }
        Task<string> GetUserRoleAsync();
        Task<EmployeeRoleTypeModel> GetUserRoleTypeAsync();
    }
}
