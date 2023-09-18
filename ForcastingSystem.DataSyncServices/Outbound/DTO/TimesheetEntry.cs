namespace ForecastingSystem.DataSyncServices.Outbound
{
    public class TimesheetEntry
    {
        public int TimesheetId { get; set; }

        public string Username { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string PhaseCode { get; set; }

        public double Hours { get
            {
                return EndDate.Subtract(StartDate).TotalHours;
            }
        }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public string RateName { get; set; }
        public double RateAmount { get; set; }
        public int RateID { get; set; }
    }
}
