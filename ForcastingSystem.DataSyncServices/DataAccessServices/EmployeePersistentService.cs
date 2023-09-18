using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.Domain.Common;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Models;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ForecastingSystem.DataSyncServices.DataAccessServices
{
    public class EmployeePersistentService : IEmployeePersistentService
    {
        private readonly ISyncEmployeeRepository _employeeRepository;
        private readonly ISyncEmployeeSkillsetRepository _employeeSkillsetRepository;
        private readonly ISyncSkillsetRepository _skillsetRepository;
        private readonly ISyncSalaryRepository _salaryRepository;
        private readonly IForecastingSystemEncryptor _forecastingSystemEncryptor;
        private readonly IDataSyncServiceLogger _dataSyncLogger;
        private readonly ISyncUserIdLookupRepository _userIdLookupRepository;
        public EmployeePersistentService(ISyncEmployeeRepository employeeRepository , ISyncSalaryRepository salaryRepository
            , ISyncEmployeeSkillsetRepository employeeSkillsetRepository , ISyncSkillsetRepository skillsetRepository
            , IForecastingSystemEncryptor forecastingSystemEncryptor ,
            IDataSyncServiceLogger dataSyncLogger , ISyncUserIdLookupRepository userIdLookupRepository)
        {
            _employeeRepository = employeeRepository;
            _salaryRepository = salaryRepository;
            _employeeSkillsetRepository = employeeSkillsetRepository;
            _skillsetRepository = skillsetRepository;
            _forecastingSystemEncryptor = forecastingSystemEncryptor;
            _dataSyncLogger = dataSyncLogger;
            _userIdLookupRepository = userIdLookupRepository;
        }

        public Task SaveEmployee(Employee employee)
        {
            UpdateSalaryIfRecordExistedInDb(employee);
            InsertToUserIdLookup(employee.Email , employee.FirstName , employee.LastName);

            var dbEmployee = _employeeRepository.FirstOrDefaultAsync(x => x.UserName == employee.UserName).Result;

            if (dbEmployee == null)
            {
                //This employee might have employeeSkillset reference to a existing skillset created by sync another employee
                LinkToExistedSkillsetIfExistedInDb(employee);
                return AddEmployee(employee);
            }
            else if (dbEmployee.ExternalId != employee.ExternalId)
            {
                string errorMessage = $"Employee {employee.FullName} has externalId {employee.ExternalId} while an existing record in db for {employee.FullName} has externalId {dbEmployee.ExternalId}";
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve ,
                    WriteIndented = true
                };

                string employeeJsonString = JsonSerializer.Serialize(employee , options);
                _dataSyncLogger.LogInfo($"Debuging data for employee {employee.FullName}:{employeeJsonString}");

                _dataSyncLogger.LogWarning($"Syncing employee {employee.FullName} Failed - {errorMessage}");
                return Task.CompletedTask;
            }
            else
            {
                employee.EmployeeId = dbEmployee.EmployeeId;
                var existingEmployeeSkillsets = _employeeSkillsetRepository.GetAsync(e => e.EmployeeId == dbEmployee.EmployeeId).Result;
                if (existingEmployeeSkillsets != null)
                {
                    // Update skillset for this employee
                    UpdateEmployeeSkillsetIfExistedIndb(employee , existingEmployeeSkillsets);
                }
                else
                {
                    LinkToExistedSkillsetIfExistedInDb(employee);
                }
                return UpdateEmployee(employee);
            }
        }

        private void InsertToUserIdLookup(string email , string? firstName , string? lastName)
        {
            //insert if not existed
            //correct email, firstname or last name if existed
            var foundByEmail = _userIdLookupRepository.FirstOrDefaultAsync(x => x.BambooHREmail == email).Result;
            var foundByName = _userIdLookupRepository.FirstOrDefaultAsync(x => x.BambooHRFirstName == firstName
            && x.BambooHRLastName == lastName).Result;
            if (foundByEmail != null || foundByName != null)
            {
                //update if newer
                var dbUserIdLookup = foundByEmail ?? foundByName;
                if (dbUserIdLookup.BambooHREmail != email
                    || dbUserIdLookup.BambooHRFirstName != firstName
                    || dbUserIdLookup.BambooHRLastName != lastName)
                {
                    dbUserIdLookup.BambooHRFirstName = firstName;
                    dbUserIdLookup.BambooHRLastName = lastName;
                    dbUserIdLookup.BambooHREmail = email;
                    dbUserIdLookup.LastUpdatedBy = Constants.DataSyncSources.BambooHRSource;
                    dbUserIdLookup.LastUpdatedDateTime = DateTime.UtcNow;
                    _userIdLookupRepository.SaveChangesAsync().Wait();
                }
            }
            else
            {
                //insert
                _userIdLookupRepository.AddAsync(new UserIdLookup()
                {
                    BambooHRFirstName = firstName ,
                    BambooHRLastName = lastName ,
                    BambooHREmail = email ,
                    LastUpdatedBy = Constants.DataSyncSources.BambooHRSource ,
                    LastUpdatedDateTime = DateTime.UtcNow ,
                }); ;
                _userIdLookupRepository.SaveChangesAsync().Wait();
            }

        }

        private static void UpdateEmployeeSkillsetIfExistedIndb(Employee employee , IEnumerable<EmployeeSkillset> existingEmployeeSkillsets)
        {
            foreach (var empSkillset in employee.EmployeeSkillsets)
            {
                var existingEmpSkillset = existingEmployeeSkillsets.FirstOrDefault(x => x.ExternalId == empSkillset.ExternalId);
                if (existingEmpSkillset != null)
                {
                    //Just add link to the primary key here, other fields was set by data mapping earlier
                    // Entity framework would take care of the update when saved
                    empSkillset.EmployeeSkillsetId = existingEmpSkillset.EmployeeSkillsetId;
                    empSkillset.Skillset.SkillsetId = existingEmpSkillset.SkillsetId;
                }
            }
        }

        private void LinkToExistedSkillsetIfExistedInDb(Employee employee)
        {
            foreach (var empSkillset in employee.EmployeeSkillsets)
            {
                var existingSkillset = _skillsetRepository.FirstOrDefaultAsync(skill => skill.SkillsetName == empSkillset.Skillset.SkillsetName).Result;
                if (existingSkillset != null)
                {
                    empSkillset.SkillsetId = existingSkillset.SkillsetId;
                }
            }
        }

        private void UpdateSalaryIfRecordExistedInDb(Employee employee)
        {
            foreach (var salary in employee.Salaries)
            {
                salary.Salary1 = ApplyOfuscation(salary.Salary1);
                var existingSalary = _salaryRepository.FirstOrDefaultAsync(x => x.ExternalId == salary.ExternalId).Result;
                if (existingSalary != null)
                {
                    //Just add link to the primary key here, other fields was set by data mapping earlier
                    // Entity framework would take care of the update when saved
                    salary.SalaryId = existingSalary.SalaryId;
                }
            }
        }

        private string? ApplyOfuscation(string? salary)
        {
            if (salary == null) return null;
            string encryptedVal = _forecastingSystemEncryptor.Encrypt(salary);

            //Decypt like this if we want to display it back to UI
            string decyptedVal = _forecastingSystemEncryptor.Decrypt(encryptedVal);

            return encryptedVal;
        }

        private Task UpdateEmployee(Employee employee)
        {
            _employeeRepository.UpdateAsync(employee).Wait();
            _employeeRepository.SaveChangesAsync().Wait();
            return Task.CompletedTask;
        }

        private Task AddEmployee(Employee employee)
        {
            _employeeRepository.AddAsync(employee).Wait();
            _employeeRepository.SaveChangesAsync().Wait();
            return Task.CompletedTask;
        }

        public void SetEmployeeInactive(Employee inactiveEmployee)
        {
            var dbEmployee = _employeeRepository.FirstOrDefaultAsync(x => x.UserName == inactiveEmployee.UserName).Result;

            if (dbEmployee == null || dbEmployee.ExternalId != inactiveEmployee.ExternalId)
            {
                // Ignore if inactive employee does not existed
            }
            else
            {
                inactiveEmployee.EmployeeId = dbEmployee.EmployeeId;
                UpdateEmployee(inactiveEmployee);
            }
        }

        public void SetEmployeeInactive(string employeeExternalId)
        {
            int externalId = int.Parse(employeeExternalId);
            var dbEmployee = _employeeRepository.FirstOrDefaultAsync(x => x.ExternalId == externalId).Result;

            if (dbEmployee == null )
            {
                // Ignore if inactive employee does not existed
            }
            else
            {
                dbEmployee.ActiveStatus = false;
                UpdateEmployee(dbEmployee);
            }
        }
    }
}
