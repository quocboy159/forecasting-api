using ForecastingSystem.Application.Common;
using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Application.Models;
using ForecastingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ForecastingSystem.Application.Services
{
    public class ProjectRevenueCalculatorService : IProjectRevenueCalculatorService
    {
        private readonly List<string> VN = new List<string> { "Viet Nam", "VN" };
        private readonly List<string> NZ = new List<string> { "New Zealand", "NZ" };
        private const int WorkingDays = 5;
        private const string Error_NoCountry_Found = "Resource data is invalid in Country field.";
        private const string StatutoryCode = "LVSTA";

        public List<ProjectRevenueModel> GetProjectRevenues(List<Project> projects, List<PublicHoliday> holidays, List<EmployeeLeave> leaves, List<UserIdLookup> userIdsLookup, bool mustRecalculate = false)
        {
            var projectRevenues = new List<ProjectRevenueModel>();         
            foreach (var project in projects) {

                //Force recalculation to have revenue by weeeks for displaying in Revenue screen
                if (mustRecalculate || project.IsObsoleteProjectValue == null || project.IsObsoleteProjectValue.Value == true)
                {
                    foreach (var phase in project.Phases)
                    {                
                        phase.PhaseValue = null;
                    }
                }           
             
                var projectRevenue = GetProjectRevenue(project, holidays, leaves, userIdsLookup);
                projectRevenues.Add(projectRevenue);
            }
            return projectRevenues;
        }

        public ProjectRevenueModel GetProjectRevenue(Project project, List<PublicHoliday> holidays, List<EmployeeLeave> leaves, List<UserIdLookup> userIdsLookup)
        {
            var projectRevenue = new ProjectRevenueModel()
            {
                ProjectId = project.ProjectId,
                ExternalProjectId = project.ExternalProjectId,
                ProjectName = project.ProjectName,
                ProjectCode = project.ProjectCode,
                ClientName = project.Client?.ClientName,
                ClientCode = project.Client?.ClientCode,
                Confident = project.Confident,
                ProjectManagerName = string.Join("; ", project.ProjectEmployeeManagers?.Select(x => x.Employee.FullName)),
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ProjectType = project.ProjectType,
            };
            foreach (var phase in project.Phases)
            {

                if (phase.PhaseResources != null && phase.PhaseResources.Any())
                {
                    //if phase value saved in db, just return it
                    if (phase.PhaseValue != null)
                    {
                        projectRevenue.PhaseRevenues.Add(new PhaseRevenueModel()
                        {
                            PhaseId = phase.PhaseId,
                            PhaseName = phase.PhaseName,
                            PhaseCode = phase.PhaseCode,
                            StartDate = phase.StartDate,
                            Budget = phase.Budget,
                            DbPhaseValue = (float)phase.PhaseValue,
                            EstimatedEndDate = phase.EstimatedEndDate,
                        });
                    }
                    else //calculate phase value again
                    {
                        var resourceEmails = phase.PhaseResources.Where(s => s.Employee != null).Select(s => s.Employee.Email).ToList();
                        var userIdsLookupNarrow = userIdsLookup.Where(s => resourceEmails.Contains(s.BambooHREmail) && !string.IsNullOrEmpty(s.TimesheetUserName)).ToList();
                        var timesheetUserNames = userIdsLookupNarrow.Select(s => s.TimesheetUserName.ToLower()).ToList();
                        var phaseResourceLeavesNarrow = leaves.Where(s => timesheetUserNames.Contains(s.TimesheetUsername.ToLower())).ToList();
                        var phaseRevenue = GetPhaseRevenue(phase, holidays, phaseResourceLeavesNarrow, userIdsLookupNarrow);                                               
                       
                        projectRevenue.PhaseRevenues.Add(phaseRevenue);

                        // considering saving
                        projectRevenue.HasChangedProjectValue = true;
                        phaseRevenue.HasChangedPhaseValue = true;
                    }
                }
                else
                {
                    projectRevenue.PhaseRevenues.Add(new PhaseRevenueModel()
                    {
                        PhaseId = phase.PhaseId,
                        PhaseName = phase.PhaseName,
                        PhaseCode = phase.PhaseCode,
                        StartDate = phase.StartDate,
                        Budget = phase.Budget
                    });
                }
            }
            return projectRevenue;
        }      

        private (bool valid, string meesage) PreValidateBeforeCalculatingRevenue(Phase phase)
        {
            bool valid = true;
            string message = null;
            if (phase.PhaseResources.Any(s => (s.Employee != null && string.IsNullOrWhiteSpace(s.Employee.Country)) ||
                                             (s.ResourcePlaceHolder != null && string.IsNullOrWhiteSpace(s.ResourcePlaceHolder.Country))))
            {
                valid = false;
                message = Error_NoCountry_Found;
            }
            return (valid, message);
        }

        public PhaseRevenueModel GetPhaseRevenue(Phase phase, List<PublicHoliday> holidays, List<EmployeeLeave> leaves, List<UserIdLookup> userIdsLookup)
        {
            var modelReturn = new PhaseRevenueModel()
            {
                PhaseId = phase.PhaseId,
                PhaseName = phase.PhaseName,
                PhaseCode = phase.PhaseCode,
                StartDate = phase.StartDate,
                EndDate = phase.EndDate,
                Budget = phase.Budget,
                IsCalculatingByResource = phase.IsCalculatingByResource,
            };
            var (valid, message) = PreValidateBeforeCalculatingRevenue(phase);
            if (!valid)
            {
                modelReturn.Error = message;
                return modelReturn;
            }

            try
            {
                phase = ClonePhaseToAvoidTrackedEFEntities(phase);
                if (!phase.StartDate.HasValue
                    || (phase.IsCalculatingByResource == true && !phase.EndDate.HasValue)
                    || (phase.IsCalculatingByResource != true && !phase.Budget.HasValue))
                    return modelReturn;

                var vnHolidays = holidays.Where(s => VN.Contains(s.Country)).Select(s => s.Date.Value).ToList();
                var nzHolidays = holidays.Where(s => NZ.Contains(s.Country)).Select(s => s.Date.Value).ToList();
                var vnResources = GetPhaseResourcesByCountry(phase.PhaseResources, VN);
                var nzResources = GetPhaseResourcesByCountry(phase.PhaseResources, NZ);    

                if (!vnResources.Any() && !nzResources.Any()) { return modelReturn; }
                var date = phase.StartDate.Value.Date;
                float weekValue = 0f, impactStatDaysWeekValue = 0f, impactLeaveWeekValue = 0f;
                float monthValue = 0f, impactStatDaysMonthValue = 0f, impactLeaveMonthValue = 0f;
                var costPerDaysMonth = new Dictionary<DateTime, float>();
                var phaseValue = 0f;
                DateTime stopDate = DateTime.MinValue;
                float buffer = 0;

                do //loop day by day
                {
                    var vnCostPerDay = CalculateCostPerDay(vnResources, date);
                    var nzCostPerDay = CalculateCostPerDay(nzResources, date);                    

                    if (IsWeekend(date))
                    {
                        modelReturn.RevenueByWeeks.Add(PhaseRevenueDetailModel.CreateWeekRevenue(date, weekValue, impactStatDaysWeekValue, impactLeaveWeekValue));
                        weekValue = impactStatDaysWeekValue = impactLeaveWeekValue = 0f;

                        if (IsEndOfMonth(date) || IsEndOfMonth(date.AddDays(1))) // end of month on Sat or Sun
                        {
                            modelReturn.RevenueByMonths.Add(PhaseRevenueDetailModel.CreateMonthRevenue(date, monthValue, impactStatDaysMonthValue, impactLeaveMonthValue, costPerDaysMonth));
                            monthValue = impactStatDaysMonthValue = impactLeaveMonthValue = 0f;
                            costPerDaysMonth = new Dictionary<DateTime, float>();
                        }
                        date = date.NextWeekMonday();//  date.AddDays((DayOfWeek.Monday - date.DayOfWeek + 7) % 7); // go forward to Monday of next week
                    }
                    else
                    {
                        // working days, reset value for day, update value for week
                        var dayValue = 0f;
                        if (!nzHolidays.Contains(date))
                        {
                            dayValue += nzCostPerDay;
                            var exceptionValue = CalculateRemainingCostPerDayAfterException(nzResources, date);
                            dayValue += exceptionValue;
                        }
                        else
                        {
                            impactStatDaysWeekValue += nzCostPerDay;
                            impactStatDaysMonthValue += nzCostPerDay;
                        }

                        if (!vnHolidays.Contains(date))
                        {
                            dayValue += vnCostPerDay;
                            var exceptionValue = CalculateRemainingCostPerDayAfterException(vnResources, date);
                            dayValue += exceptionValue;
                        }
                        else
                        {
                            impactStatDaysWeekValue += vnCostPerDay;
                            impactStatDaysMonthValue += vnCostPerDay;
                        }
                        var leaveValue = CalculateRemainingCostPerDayAfterLeaves(phase.PhaseResources.Where(s => s.EmployeeId.HasValue).ToList(), date, leaves, userIdsLookup);
                        dayValue += leaveValue;
                        impactLeaveWeekValue += leaveValue;
                        impactLeaveMonthValue += leaveValue;

                        weekValue += dayValue;
                        monthValue += dayValue;
                        phaseValue += dayValue;
                        costPerDaysMonth.Add(date, dayValue);
                        stopDate = date;

                        if (IsEndOfMonth(date))
                        {
                            modelReturn.RevenueByMonths.Add(PhaseRevenueDetailModel.CreateMonthRevenue(date, monthValue, impactStatDaysMonthValue, impactLeaveMonthValue, costPerDaysMonth));
                            monthValue = impactStatDaysMonthValue = impactLeaveMonthValue = 0f;
                            costPerDaysMonth = new Dictionary<DateTime, float>();
                        }

                        var impactDetails = GetImpactDetailsByDate(phase.PhaseResources, leaves, holidays, date, userIdsLookup);
                        if (impactDetails.Any())
                        {
                            modelReturn.ImpactDetails.AddRange(impactDetails);
                        }                        

                        date = date.AddDays(1); // next day
                    }
                    // only take buffer value when at least phaseValue is counted, to prevent few first days it's still $0 value
                    if (buffer == 0 && phaseValue > 0) buffer = vnCostPerDay + nzCostPerDay;
                }
                while ((phase.IsCalculatingByResource == true && date <= phase.EndDate.Value)
                         || (phase.IsCalculatingByResource != true && phaseValue <= (float)phase.Budget.Value - buffer) && date < new DateTime(3000, 1, 1));

                if (!IsWeekend(stopDate))
                {
                    // so just add this partial week to be last week
                    modelReturn.RevenueByWeeks.Add(PhaseRevenueDetailModel.CreateWeekRevenue(stopDate, weekValue, impactStatDaysWeekValue, impactLeaveWeekValue));
                }
                if (!IsEndOfMonth(stopDate))
                {
                    // so just add this partial month to be last month
                    modelReturn.RevenueByMonths.Add(PhaseRevenueDetailModel.CreateMonthRevenue(stopDate, monthValue, impactStatDaysMonthValue, impactLeaveMonthValue, costPerDaysMonth));
                }
                modelReturn.EstimatedEndDate = stopDate;

                if (phase.IsCalculatingByResource == true)
                {
                    modelReturn.Budget = (decimal)modelReturn.PhaseValue;
                }
            }
            catch (Exception ex)
            {
                modelReturn.Error = ex.Message;
            }

            return modelReturn;
        }

        private List<PhaseImpactDetailModel> GetImpactDetailsByDate(ICollection<PhaseResource> phaseResources, List<EmployeeLeave> leaves, List<PublicHoliday> holidays, DateTime date, List<UserIdLookup> userIdsLookup)
        {      
            var impactDetails = new List<PhaseImpactDetailModel>();

            foreach (var resource in phaseResources)
            {
                var rate = GetRate(resource, date);
                var costPerDay = CostPerDay(resource.HoursPerWeek, rate);
                var impactHours = (float)(resource.HoursPerWeek / WorkingDays);
                var offHours = 8;

                if(resource.Employee != null)
                {
                    var timesheetUserName = userIdsLookup.Where(s => resource.Employee.Email == s.BambooHREmail).Select(s => s.TimesheetUserName).FirstOrDefault();
                    if (timesheetUserName != null)
                    {
                        var leaveImpactDetails = GetLeaveImpactDetail(resource, leaves, rate, date, impactHours, costPerDay, offHours);
                        if (leaveImpactDetails.Any())
                        {
                            impactDetails.AddRange(leaveImpactDetails);
                        }
                    }
                }                              

                var statImpactDetail = GetStatutotyImpactDetail(resource, holidays, date, impactHours, costPerDay, offHours);
                if (statImpactDetail != null)
                {
                    impactDetails.Add(statImpactDetail);
                }                
            }

            return impactDetails;
        }

        private List<PhaseImpactDetailModel> GetLeaveImpactDetail(PhaseResource resource, List<EmployeeLeave> leaves, ProjectRateHistory rate, DateTime date, float impactHours, float costPerDay, int offHours)
        {
            var leaveImpactDetails = new List<PhaseImpactDetailModel>();
            var leavesByEmployee = leaves.Where(x => x.TimesheetUsername.Trim().ToLower() == resource.Employee.UserName.Trim().ToLower()).ToList();
            var leavesByDate = leavesByEmployee.Where(x => (x.EndDate == DateTime.MinValue && x.StartDate.Date == date.Date) || (x.EndDate != DateTime.MinValue && x.StartDate.Date <= date.Date && date.Date <= x.EndDate.Date)).ToList();

            if (leavesByDate.Any() && resource.Employee != null)
            {
                var leaveCosts = GetLeaveCostPerDay(leavesByDate, resource, date, impactHours);

                if (leavesByDate.Count == 2) //take 2 half leaves
                {
                    impactHours /= 2;
                }

                foreach (var leave in leavesByDate)
                {
                    leaveImpactDetails.Add(new PhaseImpactDetailModel
                    {
                        EmployeeName = resource.Employee.FullName,
                        ImpactDate = date,
                        ImpactCode = leave.LeaveCode,
                        ImpactHours = impactHours,
                        ImpactCost = leaveCosts.First(x => x.Key == leave.EmployeeLeaveId).Value,
                        OffHours = offHours
                    });
                }
            }

            return leaveImpactDetails;
        }

        private PhaseImpactDetailModel GetStatutotyImpactDetail(PhaseResource resource, List<PublicHoliday> holidays, DateTime date, float impactHours, float costPerDay, int offHours)
        {
            var holidaysByDate = holidays.Where(x => x.Date == date.Date).ToList(); //Statutory days
            var country = resource.Employee != null ? resource.Employee.Country : resource.ResourcePlaceHolder.Country;
            var holiday = VN.Contains(country) ?
                 holidaysByDate.FirstOrDefault(x => x.Date == date.Date && VN.Contains(x.Country))
                 : holidaysByDate.FirstOrDefault(x => x.Date == date.Date && NZ.Contains(x.Country));

            if (holiday != null)
            {
                return new PhaseImpactDetailModel
                {
                    EmployeeName = resource.Employee != null ? resource.Employee.FullName : resource.ResourcePlaceHolder.ResourcePlaceHolderName,
                    ImpactDate = date,
                    ImpactCode = StatutoryCode,
                    StatutoryName = holiday.Name,
                    ImpactHours = impactHours,
                    ImpactCost = costPerDay,
                    OffHours = offHours
                };
            }

            return null;
        }

        private Phase ClonePhaseToAvoidTrackedEFEntities(Phase phase)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(phase, options);
            var deserializedPhase = JsonSerializer.Deserialize<Phase>(jsonString, options);
            phase = deserializedPhase;
            return phase;
        }

        private bool IsEndOfMonth(DateTime date)
        {
            return date.Day == DateTime.DaysInMonth(date.Year, date.Month);
        }
        private bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday ||
                         date.DayOfWeek == DayOfWeek.Sunday;
        }
        private float CalculateRemainingCostPerDayAfterLeaves(List<PhaseResource> phaseResources, DateTime date, List<EmployeeLeave> leaves, List<UserIdLookup> userIdsLookup)
        {
            var result = 0f;
            foreach (var phaseResource in phaseResources)
            {
                if(phaseResource.Employee != null)
                {
                    var timesheetUserName = userIdsLookup.Where(s => phaseResource.Employee.Email == s.BambooHREmail).Select(s => s.TimesheetUserName).FirstOrDefault();
                    if (timesheetUserName != null)
                    {
                        var resourceLeaves = leaves.Where(s => s.TimesheetUsername.ToLower() == timesheetUserName?.ToLower()).ToList();
                        var leavesByDate = resourceLeaves.Where(x => (x.EndDate == DateTime.MinValue && x.StartDate.Date == date.Date) || (x.EndDate != DateTime.MinValue && x.StartDate.Date <= date.Date && date.Date <= x.EndDate.Date)).ToList();
                        var hours = (float)(phaseResource.HoursPerWeek / WorkingDays);

                        var costs = GetLeaveCostPerDay(leavesByDate, phaseResource, date, hours);
                        result -= costs.Sum(x => x.Value);
                    }
                }                
            }
            return result;
        }

        private Dictionary<int, float> GetLeaveCostPerDay(List<EmployeeLeave> leaves, PhaseResource phaseResource, DateTime date, float hours)
        {            
            var leaveCosts = new Dictionary<int, float>();          

            if (leaves.Count == 2) //take 2 half leaves
            {
                hours = hours / 2;
            }

            foreach (var leave in leaves)
            {
                var rate = GetRate(phaseResource, date);               
                if (leave.EndDate == DateTime.MinValue && hours > 4)
                {
                    hours = 4;
                }

                var costPerDay = (hours * (float)(rate?.Rate ?? 0));
                leaveCosts.Add(leave.EmployeeLeaveId, costPerDay);              
            }

            return leaveCosts;
        }

        private List<PhaseResource> GetPhaseResourcesByCountry(ICollection<PhaseResource> phaseResources, List<string> country)
        {
            return phaseResources.Where(s => (s.Employee != null && country.Contains(s.Employee.Country)) ||
                                             (s.ResourcePlaceHolder != null && country.Contains(s.ResourcePlaceHolder.Country)))
                                 .ToList();
        }

        private float CalculateRemainingCostPerDayAfterException(List<PhaseResource> phaseResources, DateTime date)
        {
            var result = 0f;
            foreach (var phaseResource in phaseResources)
            {
                foreach (var exception in phaseResource.PhaseResourceExceptions)
                {
                    if (exception.StartWeek <= date && date <= exception.StartWeek.AddDays(exception.NumberOfWeeks * 7))
                    {
                        var rate = GetRate(phaseResource, date);
                        float costException = CostPerDay(exception.HoursPerWeek, rate);
                        float employeeCostPerDay = CostPerDay(phaseResource.HoursPerWeek, rate);
                        result += (costException - employeeCostPerDay);
                    }
                }
            }
            return result;
        }
        private float CalculateCostPerDay(List<PhaseResource> phaseResource, DateTime date)
        {
            var costPerDay = 0d;
            foreach (var res in phaseResource)
            {
                var rate = GetRate(res, date);
                costPerDay += CostPerDay(res.HoursPerWeek, rate);
            }
            return (float)costPerDay;
        }

        private float CostPerDay(double hoursPerWeek, ProjectRateHistory rate)
        {
            var costPerDay = ((float)hoursPerWeek / WorkingDays) * (float)(rate?.Rate ?? 0);
            return costPerDay;
        }
        private float CostPerDay(double hoursPerWeek, double? rateAmount)
        {
            var costPerDay = ((float)hoursPerWeek / WorkingDays) * (float)(rateAmount ?? 0);
            return costPerDay;
        }

        private ProjectRateHistory GetRate(PhaseResource phaseResource, DateTime date)
        {
            var histories = phaseResource.ProjectRate.ProjectRateHistories
                                    .OrderBy(s => s.StartDate).ToList();
            DateTime start;
            DateTime end;
            for (var idx = 0; idx < histories.Count; idx++)
            {
                start = histories[idx].StartDate.Value.Date;
                end = idx < histories.Count - 1 ? histories[idx + 1].StartDate.Value.Date : DateTime.MaxValue;
                if (start <= date && date < end)
                {
                    return histories[idx];
                }
            }
            return null;
        }

        public List<ProjectPhaseActualRevenueModel> GetActualProjectRevenues(List<EmployeeTimesheetEntry> timesheetEntries, DateTime startWeek, DateTime currentWeek)
        {
            var modelReturn = new List<ProjectPhaseActualRevenueModel>();
            var projectPhaseGroup = timesheetEntries.GroupBy(s => new { s.ExternalProjectId, s.ProjectName, s.PhaseCode }).ToList();
            foreach (var projectPhase in projectPhaseGroup)
            {
                var projectPhases = new ProjectPhaseActualRevenueModel()
                {
                    ProjectName = projectPhase.Key.ProjectName,
                    ExternalProjectId = projectPhase.Key.ExternalProjectId,
                    PhaseCode = projectPhase.Key.PhaseCode
                };
                var entries = projectPhase.ToList();
                var date = startWeek;
                while (date < currentWeek) // actual week
                {
                    var startDateTime = date.Date;
                    var endDateTime = date.NextWeekMonday().EndOfPrevDateTime();
                    var entriesInWeek = entries.Where(e => startDateTime <= e.StartDate && e.EndDate <= endDateTime).ToList();
                    if (entriesInWeek.Count > 0)
                    {
                        var weekValue = (float)entriesInWeek.Sum(e => e.Hours * e.RateAmount);
                        projectPhases.RevenueByWeeks.Add(PhaseRevenueDetailModel.CreateWeekRevenue(date , weekValue , 0 , 0));
                    }
                    date = date.NextWeekMonday();
                }

                date = startWeek;
                while (date < currentWeek) // actual Month
                {
                    var startDateTime = date.FirstDayOfMonth();
                    var endDateTime = new DateTime(Math.Min(date.EndDateTimeOfMonth().Ticks, currentWeek.EndOfPrevDateTime().Ticks));
                    var entriesInMonth = entries.Where(e => startDateTime <= e.StartDate && e.EndDate <= endDateTime).ToList();
                    if (entriesInMonth.Count > 0)
                    {
                        var monthValue = (float)entriesInMonth.Sum(e => e.Hours * e.RateAmount);
                        projectPhases.RevenueByMonths.Add(PhaseRevenueDetailModel.CreateMonthRevenue(startDateTime , endDateTime , monthValue , 0 , 0));
                    }
                    date = date.FirstDayOfNextMonth();
                }

                modelReturn.Add(projectPhases);
            }
            return modelReturn;
        }
    }
}
