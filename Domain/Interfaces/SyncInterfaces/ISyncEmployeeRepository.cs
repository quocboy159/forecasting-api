using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface ISyncEmployeeRepository : IAsyncRepository<Employee>
    {
    }
}
