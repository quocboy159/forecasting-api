namespace ForecastingSystem.DataSyncServices.Outbound

{
    public class BHRMetaList
    {
        public int FieldId { get; set; }
        public string Manageable { get; set; }
        public string Multiple { get; set; }
        public string Name { get; set; }
        public List<BHROption> Options { get; set; }
        public string Alias { get; set; }
    }

}
