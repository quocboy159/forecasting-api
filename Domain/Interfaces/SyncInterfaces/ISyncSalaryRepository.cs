using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface ISyncSalaryRepository : IAsyncRepository<Salary>
    {
    }
}
