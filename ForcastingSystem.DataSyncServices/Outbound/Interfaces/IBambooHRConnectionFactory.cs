namespace ForecastingSystem.DataSyncServices.Outbound.Interfaces
{
    public interface IBambooHRConnectionFactory
    {
        IBambooHRApi GetBambooHrHttpClient();
    }
}
