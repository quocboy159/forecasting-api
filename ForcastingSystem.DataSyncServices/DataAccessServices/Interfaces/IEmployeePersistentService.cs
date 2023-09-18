using ForecastingSystem.Domain.Models;

namespace ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces
{
    public interface IEmployeePersistentService
    {
        Task SaveEmployee(Employee employee);
        void SetEmployeeInactive(Employee inactiveFsEmployee);
        void SetEmployeeInactive(string employeeExternalId);
    }
}
