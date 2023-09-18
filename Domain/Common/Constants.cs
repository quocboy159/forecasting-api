namespace ForecastingSystem.Domain.Common
{
    public static class Constants
    {
        public static class ProjectType
        {
            public static string Opportunity = "Opportunity";
            public static string Project = "Project";
        }

        public static class DataSyncTypes
        {
            public static string TimesheetSyncEntryJob = "TimesheetSyncEntryJob";
            public static string TimesheetSyncDeletedEntryJob = "TimesheetSyncDeletedEntryJob";
            public static string TimesheetSyncProjectJob = "TimesheetSyncProjectJob";
            public static string TimesheetSyncClientJob = "TimesheetSyncClientJob";
            public static string TimesheetSyncPhaseJob = "TimesheetSyncPhaseJob";
            public static string TimesheetSyncProjectRateJob = "TimesheetSyncProjectRateJob";
            public static string TimesheetSyncProjectRateHistoryJob = "TimesheetSyncProjectRateHistoryJob";
            public static string TimesheetSyncEmployeeLeaveJob = "TimesheetSyncEmployeeLeaveJob";
            public static string TimesheetSyncPublicHolidayJob = "TimesheetSyncPublicHolidayJob";
            public static string TimesheetSyncEmployeeJob = "TimesheetSyncEmployeeJob";

            


            public static string LeaveSystemSyncJob = "LeaveSystemSyncJob";

            public static string BambooHRSyncEmployeeDetailJob = "BambooHRSyncEmployeeDetailJob";
            public static string BambooHRSyncSkillsJob = "BambooHRSyncSkillsJob";

            
        }

        public static class DataSyncSources
        {
            public static string TimesheetSource = "Timesheet";
            public static string LeaveSystemSource = "LeaveSystem";
            public static string BambooHRSource = "BambooHR";
        }

        public static class DataSyncTargets
        {
            public static string ForecastTarget = "Forecast";
        }

        public static class SkillsetCategories
        {
            public static string Default = "Other";
        }

        public static class BambooHRMetaTypes
        {
            public static string Skill = "Skill";
        }

        public static class UserRole
        {
            public static string Admin = "Admin";
            public static string ProjectManager = "ProjectManager";
        }

        public static class SkillsetProficiencyLevel
        {
            public const string Beginer = "Beginer";
            public const string Intermediate = "Intermediate";
            public const string Advanced = "Advanced";
            public const string Expert = "Expert";
            public const string Master = "Master";
        }
    }
}
