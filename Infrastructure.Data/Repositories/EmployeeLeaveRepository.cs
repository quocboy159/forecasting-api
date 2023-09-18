using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class EmployeeLeaveRepository : GenericAsyncRepository<EmployeeLeave>, IEmployeeLeaveRepository
    { 
        public EmployeeLeaveRepository(ForecastingSystemDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<EmployeeLeave>> GetLeavesAsync(DateTime? fromDate = null, List<string> timesheetUsernames = null)
        {
            var daysQuery = DbContext.Set<EmployeeLeave>().AsNoTracking();
            if (fromDate.HasValue)
            {
                daysQuery = daysQuery.Where(s => s.StartDate >= fromDate.Value.Date);
            }
            if (timesheetUsernames != null && timesheetUsernames.Any())
            {
                daysQuery = daysQuery.Where(s => timesheetUsernames.Contains(s.TimesheetUsername));
            }
            return daysQuery.ToListAsync();
        }

        public Task<List<EmployeeLeave>> GetLeavesLastSevenMonthBy(string username)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            var date = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            DateTime toDate = date.ToUniversalTime();
            var temp = date.AddMonths(-7);
            DateTime fromDate = (new DateTime(temp.Year, temp.Month, 1)).ToUniversalTime();

            var daysQuery = DbContext.Set<EmployeeLeave>().AsNoTracking().Where(x => x.TimesheetUsername == username && x.StartDate >= fromDate && x.EndDate <= toDate);

            return daysQuery.ToListAsync();
        }
    }
}
