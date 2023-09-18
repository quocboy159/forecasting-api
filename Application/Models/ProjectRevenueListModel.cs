using System;
using System.Collections.Generic;
using System.Linq;

namespace ForecastingSystem.Application.Models
{
    public class ProjectRevenueListModel
    {
        public DateTime StartWeek { get; set; }
        public List<ProjectRevenueModel> Projects { get; set; }
        public List<string> ConsumedTimes { get; set; }
        public int NoOfWeekCalculatedFromStartWeek
        {
            get
            {
                var largestWeek = Projects != null && Projects.Any() ? Projects.Max(s => s.LargestWeek) : null;
                if (!largestWeek.HasValue) return 1; // 1 week for start week
                var diff = 1 + (largestWeek.Value - StartWeek).Days / 7;
                return diff > 0 ? Math.Min(diff, 52) : 1;
            }
        }

        public int NoOfMonthCalculatedFromStartWeek
        {
            get
            {
                var largestMonth = Projects != null && Projects.Any() ? Projects.Max(s => s.LargestMonth) : null;
                if (!largestMonth.HasValue) return 1; // 1 month for start week
                var diff = 1 + ((largestMonth.Value.Year - StartWeek.Year) * 12) + largestMonth.Value.Month - StartWeek.Month;
                return diff > 0 ? Math.Min(diff, 12) : 1;
            }
        }
    }
}
