namespace ForecastingSystem.DataSyncServices.Outbound
{
    public class Project {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string ProjectType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int Confident { get; set; }
        public string Status { get; set; }
        public string ProjectManagerUserName { get; set; }
        public string ProjectManagerEmail { get; set; }

        public IList<ProjectManager> ProjectManagers { get; set; } = new List<ProjectManager>();
        public int FSClientId { get; set; }
    }

    public class ProjectManager
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
