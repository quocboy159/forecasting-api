using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.DataSyncServices.Outbound

{
    public class Employee
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PersonId { get; set; }
        public int DomainUserId { get; set; }
        public bool IsActive { get; set; }
    }
}
