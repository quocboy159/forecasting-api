using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models
{
    public class ProjectRateHistoryModel
    {
        public int ProjectRateHistoryId { get; set; }

        public double? Rate { get; set; }

        public DateTime? StartDate { get; set; }
    }
}
