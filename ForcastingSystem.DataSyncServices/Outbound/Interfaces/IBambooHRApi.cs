using Refit;

namespace ForecastingSystem.DataSyncServices.Outbound
{
    public interface IBambooHRApi
    {
        //[Get("/employees/directory?format=JSON")]
        //Task<EmployeesResult> GetEmployees();

        [Get("/employees/changed?format=JSON&since={sinceDateStr}")]
        Task<BHRLastChangedEmployees> GetChangedEmployees(string sinceDateStr);

        [Get("/employees/{employeeId}?fields=gender,firstName,lastName,dateOfBirth,middleName,preferredName,jobTitle,contractEndDate,hireDate,workPhone,mobilePhone,workEmail,department,location,division,linkedIn,pronouns,workPhoneExtension,supervisor,country,employmentStatus")]
        Task<BHREmployee> GetEmployeeById(string employeeId);


        [Get("/employees/{employeeId}/tables/employmentStatus")]
        Task<BHREmployeeEmploymentStatus[]> GetEmployeeEmploymentStatus(string employeeId);

        [Get("/employees/{employeeId}/tables/customAdditionalCompensationInfo")]
        Task<BHREmployeeAdditionalCompensation[]> GetEmployeeAdditionalCompensationInfo(string employeeId);

        [Get("/employees/{employeeId}/tables/compensation")]
        Task<BHRCompensationItem[]> GetEmployeeCompensationInfo(string employeeId);

        [Get("/employees/{employeeId}/tables/customSkills")]
        Task<BHREmployeeSkill[]> GetEmployeeSkills(string employeeId);


        [Get("/meta/lists/")]
        Task<BHRMetaList[]> GetBambooHRMetaLists();
    }
}
