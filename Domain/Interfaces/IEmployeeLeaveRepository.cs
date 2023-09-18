using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IEmployeeLeaveRepository : IAsyncRepository<EmployeeLeave>
    {
        //Task<List<EmployeeLeave>> GetLeavesAsync(DateTime? fromDate);
        //Task<List<EmployeeLeave>> GetLeavesAsync(List<string> usernames);
        Task<List<EmployeeLeave>> GetLeavesAsync(DateTime? fromDate = null, List<string> timesheetUsernames = null);
        Task<List<EmployeeLeave>> GetLeavesLastSevenMonthBy(string username);
    }
}
