using ForecastingSystem.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface ITimesheetService
    {
        Task<EmployeeRoleTypeModel> GetUserRoleTypeAsync(string username);
        Task UpdatePhaseBudgetAsync(UpdatePhaseBudgetModel updatePhaseBudgetModel);
    }
}
