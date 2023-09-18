using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.DataSync.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ForecastingSystem.DataSyncConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            IConfiguration configuration = new ConfigurationBuilder()
                           .AddJsonFile("appsettings.json" , optional: true , reloadOnChange: true)
                           .Build();

            var serviceProvider = new ServiceCollection()
                 .AddSingleton<IConfiguration>(configuration)
                .AddDataSyncServices(configuration)
                .BuildServiceProvider();

            var syncStartupService = serviceProvider.GetRequiredService<IDataSyncStartup>();
            await syncStartupService.SyncAllDataAsync(CancellationToken.None);
        }
    }
}