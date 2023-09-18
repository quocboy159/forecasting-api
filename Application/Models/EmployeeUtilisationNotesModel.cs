using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models
{
    public class EmployeeUtilisationNotesModel
    {
        public int EmployeeId { get; set; }
        public string ForecastWarning { get; set; }

        public string InternalWorkNotes { get; set; }

        public string OtherNotes { get; set; }
    }
}
