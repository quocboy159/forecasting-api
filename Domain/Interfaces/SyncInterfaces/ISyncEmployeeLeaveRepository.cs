using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.Domain.Interfaces
{
    public interface ISyncEmployeeLeaveRepository : IAsyncRepository<EmployeeLeave>
    {}
}
