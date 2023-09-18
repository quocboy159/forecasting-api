using ForecastingSystem.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sentry;
using Serilog;
using System;

namespace ForecastingSystem.BackendAPI
{
    public static class SentryHostBuilderExtensions
    {
        public static IHostBuilder UseSentry(this IHostBuilder builder) =>
            builder.ConfigureLogging((ctx , logging) =>
            {
                var sentryOptions = ctx.Configuration.GetSection("Sentry");
                SentrySdk.Init(o =>
                {
                    // Tells which project in Sentry to send events to:
                    o.Dsn = sentryOptions["Dsn"];
                    // When configuring for the first time, to see what the SDK is doing:
                    o.Debug = bool.Parse(sentryOptions["Debug"]);
                    // Set traces_sample_rate to 1.0 to capture 100% of transactions for performance monitoring.
                    // We recommend adjusting this value in production.
                    o.TracesSampleRate = 1.0;
                    // Enable Global Mode if running in a client app
                    o.IsGlobalModeEnabled = true;
                    o.Environment = sentryOptions["Environment"];
                    o.DefaultTags.Add("FSComponent" , "Backend API");
                    o.Release = Version.VersionNumber;
                });
            });
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            // Read configuration from appsetting.json
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            // Initialize logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)               
                .CreateLogger();

            try
            {
               
                var host = CreateHostBuilder(args).Build();

                SeedDatabase(host);
                SentrySdk.Flush();

                host.Run();
            }
            catch (Exception e)
            {
                Log.Error(e , "The application failed to start!");
                SentrySdk.CaptureException(e);
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
                SentrySdk.Flush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseSentry()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void SeedDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var libraryDbContext = services.GetRequiredService<ForecastingSystemDbContext>();
                    ForecastingSystemDbContextSeed.SeedAsync(libraryDbContext, loggerFactory).Wait();
                }
                catch (Exception exception)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(exception, "An error occurred seeding the DB.");
                }
            }
        }
    }
}
