using Refit;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using ForecastingSystem.DataSyncServices.Outbound.Interfaces;

namespace ForecastingSystem.DataSyncServices.Outbound
{
    public class BambooHRConnectionFactory : IBambooHRConnectionFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private IConfiguration _configuration;
        public BambooHRConnectionFactory(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public IBambooHRApi GetBambooHrHttpClient()
        {
            var bambooHRApiRoot = _configuration["BambooHRAPI:URL"] ?? string.Empty;
            var httpClient = _httpClientFactory.CreateClient("BambooHrApi");
            httpClient.BaseAddress = new Uri(bambooHRApiRoot);

            var clientId = _configuration["BambooHRAPI:APIKey"];
            var clientSecret = "BamboHrIgnoreSecret";
            var authenticationString = $"{clientId}:{clientSecret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authenticationString));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header


            var bambooHRApi = RestService.For<IBambooHRApi>(httpClient);
            return bambooHRApi;
        }
    }
}
