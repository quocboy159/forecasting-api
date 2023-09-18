using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Interfaces
{
    public interface IEmployeeService
    {     
        EmployeeListModel GetEmployees();
        Task<IEnumerable<EmployeeModel>> GetProjectResourcesAsync();
        Task<bool> IsExistingEmployeeAsync(int employeeId);

        Task<EmployeeDetailModel> GetEmployeeDetailAsync(int employeeId);

        Task SaveEmployeeUtilisationNotes(EmployeeUtilisationNotesModel model);    
    }
}
