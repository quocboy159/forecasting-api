using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models
{
    public class UpdatePhaseBudgetModel
    {
        public string UserName { get; set; }
        public int PhaseID { get; set; }
        public decimal Budget { get; set; }
    }
}
