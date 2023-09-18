using Refit;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using ForecastingSystem.DataSyncServices.Outbound.Interfaces;

namespace ForecastingSystem.DataSyncServices.Outbound
{
    public class TimesheetConnectionFactory : ITimesheetConnectionFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private IConfiguration _configuration;
        public TimesheetConnectionFactory(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public ITimesheetApi GetTimesheetHttpClient()
        {
            var timesheetApiRoot = _configuration["TimesheetAPI:URL"] ?? string.Empty;
            var httpClient = _httpClientFactory.CreateClient("TimesheetAPI");
            httpClient.BaseAddress = new Uri(timesheetApiRoot);

            var apiSecretKey = _configuration["TimesheetAPI:APIKey"];
            httpClient.DefaultRequestHeaders.Add("X-ApiKey", apiSecretKey);


            var timesheetAPI = RestService.For<ITimesheetApi>(httpClient);
            return timesheetAPI;
        }
    }
}
