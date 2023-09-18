using ForecastingSystem.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace ForecastingSystem.Application.Models
{
    public class PhaseRevenueModel
    {
        public int PhaseId { get; set; }
        public string PhaseName { get; set; }
        public string PhaseCode { get; set; }
        public bool? IsCalculatingByResource { get; set; }
        public decimal? Budget { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<PhaseRevenueDetailModel> RevenueByWeeks { get; set; } = new List<PhaseRevenueDetailModel>();
        public List<PhaseRevenueDetailModel> RevenueByMonths { get; set; } = new List<PhaseRevenueDetailModel>();
        public List<PhaseImpactDetailModel> ImpactDetails { get; set; } = new List<PhaseImpactDetailModel>();
        public List<PhaseImpactDetailModel> ImpactDetailsByWeeks { get; set; } = new List<PhaseImpactDetailModel>();
        public List<PhaseImpactDetailModel> ImpactDetailsByMonths { get; set; } = new List<PhaseImpactDetailModel>();
        public string Error { get; set; }
        public float PhaseValue
        {
            get
            {
                if (DbPhaseValue.HasValue) return DbPhaseValue.Value;
                if (RevenueByWeeks != null)
                {
                    return RevenueByWeeks.Sum(s => s.Revenue);
                }
                return 0f;
            }
        }
        public float? DbPhaseValue { get; set; } = null;
        public bool HasChangedPhaseValue { get; set; } = false;
        public DateTime? EstimatedEndDate { get; set; }

        [JsonIgnore]
        public DateTime? LargestWeek
        {
            get
            {
                return RevenueByWeeks != null && RevenueByWeeks.Any() ? RevenueByWeeks.Last().StartDate : null;
            }
        }

        [JsonIgnore]
        public DateTime? LargestMonth
        {
            get
            {
                return RevenueByMonths != null && RevenueByMonths.Any() ? RevenueByMonths.Last().StartDate : null;
            }
        }
    }
    public class PhaseRevenueDetailModel
    {
        public PhaseRevenueDetailModel() { }
        public PhaseRevenueDetailModel(float revenue, float impactStatDays, float impactLeave)
        {
            Revenue = revenue;
            ImpactStatDays = impactStatDays;
            ImpactLeave = Math.Abs(impactLeave);
        }
        public static PhaseRevenueDetailModel CreateWeekRevenue(DateTime startDate, float revenue, float impactStatDays, float impactLeave)
        {
            return new PhaseRevenueDetailModel(revenue, impactStatDays, impactLeave)
            {
                StartDate = startDate.CurrentWeekMonday() // startDate.AddDays((-1) * ((startDate.DayOfWeek - DayOfWeek.Monday + 7) % 7)) // go back to Monday of this week
            };
        }

        public static PhaseRevenueDetailModel CreateMonthRevenue(DateTime startDate, float revenue, float impactStatDays, float impactLeave, Dictionary<DateTime, float> costPerDays)
        {
            return new PhaseRevenueDetailModel(revenue, impactStatDays, impactLeave)
            {
                StartDate = startDate.FirstDayOfMonth(),
                CostPerDays = costPerDays
            };
        }

        public static PhaseRevenueDetailModel CreateMonthRevenue(DateTime startDate, DateTime endDate, float revenue, float impactStatDays, float impactLeave)
        {
            return new PhaseRevenueDetailModel(revenue, impactStatDays, impactLeave)
            {
                StartDate = startDate.FirstDayOfMonth(),
                EndDate = endDate.Date,
            };
        }

        public DateTime StartDate { get; set;}
        public DateTime? EndDate { get; set;}
        public float Revenue { get; set; }
        public float ImpactStatDays { get; set; }
        public float ImpactLeave { get; set; }
        public bool IsActual { get; internal set; }
        public Dictionary<DateTime, float> CostPerDays { get; set; } = new Dictionary<DateTime, float>();
    }

    public class PhaseImpactDetailModel
    {
        public string ImpactCode { get; set; }
        public string StatutoryName { get; set; }      
        public DateTime ImpactDate { get; set; }
        public float OffHours { get; internal set; }
        public string EmployeeName { get; set; }       
        public float ImpactCost { get; set; }
        public float ImpactHours { get; internal set; }      

        public DateTime WeekStartDate
        {
            get
            {
                return ImpactDate.CurrentWeekMonday();
            }
        }

        public DateTime MonthStartDate
        {
            get
            {
                return ImpactDate.FirstDayOfMonth();
            }
        }

    }
}
