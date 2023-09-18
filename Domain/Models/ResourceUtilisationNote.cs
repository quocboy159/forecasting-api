using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Domain.Models
{
    public class ResourceUtilisationNote
    {
        public int ResourceUtilisationNoteId { get; set; }
        public int? EmployeeId { get; set; }
        public int? ResourcePlaceHolderId { get; set; }
        public int? ProjectId { get; set; }
        public DateTime StartWeek { get; set; }
        public string Note { get; set; }
    }
}
