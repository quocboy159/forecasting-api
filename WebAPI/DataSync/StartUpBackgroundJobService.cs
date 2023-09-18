using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System;
using Hangfire;
using ForecastingSystem.DataSyncServices.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ForecastingSystem.BackendAPI.DataSync
{
    public class StartUpBackgroundJobService : BackgroundService
    {
        private const int generalDelay = 1 * 10 * 1000; // 10 seconds
        private IDataSyncStartup _dataSyncService;
        private IConfiguration _configuration;

        public StartUpBackgroundJobService(IDataSyncStartup dataSyncService, IHostApplicationLifetime appLifetime, IConfiguration configuration)
        {
            _dataSyncService = dataSyncService;
            _configuration = configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ExecuteDataSync(stoppingToken);

            // while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(generalDelay, stoppingToken);
                await CreateRecurringDataSyncJob();

            }
        }

        private void ExecuteDataSync(CancellationToken token)
        {
            RecurringJob.AddOrUpdate(() => _dataSyncService.SyncAllDataAsync(token), BuildCronExpression());
        }

        private string BuildCronExpression()
        {
            var intervalInMinutes = _configuration.GetValue<int>("DataSyncService:RecurringIntervalInMinutes");
            return $"*/{intervalInMinutes} * * * *";
        }

        private static Task CreateRecurringDataSyncJob()
        {
            RecurringJob.AddOrUpdate(() => Console.Write("Recurring ever 15 min"), "*/15 * * * *");
            return Task.FromResult($"Job ID:    15 mins!");
        }
    }
}
