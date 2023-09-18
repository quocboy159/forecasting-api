using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Application.Models
{
    public class ResourceUtilisationListModel
    {
        private int _maxNumberOfWeeks = 52;
        public DateTime StartWeek { get; set; }
        public List<ResourceUtilisationModel> Resources { get; set; }
        public int NoOfWeekCalculatedFromStartWeek
        {
            get
            {
                var largestWeek = Resources != null && Resources.Any() ? Resources.Max(s => s.LargestWeek) : null;
                if (!largestWeek.HasValue) return 1; // 1 week for start week
                var diff = 1 + (largestWeek.Value - StartWeek).Days / 7;
                return diff > 0 ? Math.Min(diff, _maxNumberOfWeeks) : 1;
            }
        }

        public List<string> ConsumedTimes { get; set; }

        public ResourceUtilisationListModel() { }

        public ResourceUtilisationListModel(int maxNumberOfWeeks) 
        {
            _maxNumberOfWeeks = maxNumberOfWeeks;
        }
    }
}
