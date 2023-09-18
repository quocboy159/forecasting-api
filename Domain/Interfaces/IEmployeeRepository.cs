using ForecastingSystem.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface IEmployeeRepository : IAsyncRepository<Employee>
    {
        Task<Employee> GetEmployeeDetailAsync(int employeeId);
        Task<List<Employee>> GetEmployeesForResourceUtilisationAsync();
    }
}
