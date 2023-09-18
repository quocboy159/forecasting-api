namespace ForecastingSystem.DataSyncServices.Outbound
{
    public class PublicHoliday
    {
        public int LeaveHolidayId { get; set; }
        public DateTime Date { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public object CalendarItemId { get; set; }
        public int Year { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
