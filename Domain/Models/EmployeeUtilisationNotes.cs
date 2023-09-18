using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Models
{
    public class EmployeeUtilisationNotes : IAuditable
    {
        public int EmployeeUtilisationNotesId { get; set; }

        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; } = null!;

        public string ForecastWarning{ get; set; }

        public string InternalWorkNotes { get; set; }

        public string OtherNotes { get; set; }
    }
}
