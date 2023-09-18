using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.Data.Repositories
{
    public class EmployeeRepository : GenericAsyncRepository<Employee>, IEmployeeRepository
    { 
        public EmployeeRepository(ForecastingSystemDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Employee> GetEmployeeDetailAsync(int employeeId)
        {
            var result = await DbContext.Employees.AsNoTracking()
                                            .Include(x => x.EmployeeSkillsets)
                                            .ThenInclude(x => x.Skillset)
                                            .ThenInclude(x => x.SkillsetCategory)
                                            .Include(x => x.EmployeeUtilisationNotes)
                                            .FirstAsync(x => x.EmployeeId == employeeId);

            return result;
        }

        public async Task<List<Employee>> GetEmployeesForResourceUtilisationAsync()
        {
            var result = await DbContext.Employees.AsNoTracking()
                .Where(s=> s.ActiveStatus == true)
                .ToListAsync();
            return result;
        }
    }
}
