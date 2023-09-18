namespace ForecastingSystem.DataSyncServices.Outbound

{
    public static class BambooHREmploymentStatusValues {
        public static string Terminated  = "Terminated";
        public static string Contractor = "Contractor";
        public static string FullTime  = "Full-Time";
        public static string PartTime  = "Part-Time";
        public static string Furloughed = "Furloughed";
    }

    public static class BambooHRAccountStatus
    {
        public static string Active = "Active";
        public static string Inactive = "Inactive";
    }

    public static class BambooHREmployeeLastChanged
    {
        public static string Inserted = "Inserted";
        public static string Updated = "Updated";
        public static string Deleted = "Deleted";
    }

}
