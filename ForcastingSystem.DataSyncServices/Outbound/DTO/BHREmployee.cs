using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.DataSyncServices.Outbound

{
    public class BHREmployee
    {
        public string Id { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public string WorkEmail { get; set; } = string.Empty;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PreferredName { get; set; }
        
        public string Gender { get; set; }
        public string JobTitle { get; set; }
        public string WorkPhone { get; set; }
        public string MobilePhone { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Division { get; set; }
        public string LinkedIn { get; set; }
        public string Pronouns { get; set; }
        public string WorkPhoneExtension { get; set; }
        public string Supervisor { get; set; }
        public string Country { get; set; }
        public string DateOfBirth { get; set; }
        public string ContractEndDate { get; set; }
        public string HireDate { get; set; }
        public string EmploymentStatus { get; set; }
        

        public override string ToString() =>
            string.Join(Environment.NewLine , $"Id: {Id}, Name: {DisplayName}, Email: {WorkEmail}");
    }

    public class Field
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class EmployeesResult
    {
        public BHREmployee[] Employees { get; set; }
        public Field[] Fields { get; set; }
    }

    public class LastChangedEmployee
    {
        public string Id { get; set; }
        public string Action { get; set; }
        public DateTime LastChanged { get; set; }
    }

    public class BHRLastChangedEmployees
    {
        public DateTime Latest { get; set; }
        public Dictionary<string,LastChangedEmployee> Employees { get; set; }
    }

}
