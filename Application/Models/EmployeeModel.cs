namespace ForecastingSystem.Application.Models
{
    public partial class EmployeeModel
    {
        public string EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }

        public string RateName { get; set; }

        public bool ActiveStatus { get; set; }

        public int PhaseResourceId { get; set; }
        public string Country { get; set; }
        public string WorkingHours { get; set; }
        public int WorkingHoursNumber { get; set; }
        public string Email { get; set; }
    }
}
