using AutoMapper.Configuration;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sentry;
using Sentry.Serilog;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Display;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace ForecastingSystem.DataSyncServices
{
    public class DataSyncServiceLogger : IDataSyncServiceLogger
    {
        IConfiguration _configuration;
        List<string> PendingMessages = new List<string>();
        public DataSyncServiceLogger(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void LogInfo(string message)
        {
            PendingMessages.Add(message);
            //Log.Information(message);
        }

        public void LogWarning(string message)
        {
            Log.Warning(message);
        }
        public void LogDebug(string message)
        {
            //PendingMessages.Add(message);
            Log.Debug(message);
        }

        public void LogError(string message , Exception ex)
        {
            Log.Error(ex , message);
        }

        public void EndReport()
        {
            Log.Information(string.Join(System.Environment.NewLine , PendingMessages));
            Log.CloseAndFlush();
            PendingMessages = new List<string>();
        }

        public void StartNewReport()
        {
            var sentryOptions = _configuration.GetSection("Sentry");
            var isLocalDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Local";

            if (isLocalDevelopment)
            {
                //Log to files
                Serilog.Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/DataSyncLog.txt" , rollingInterval: RollingInterval.Day)
                .WriteTo.File(new CompactJsonFormatter() , "Logs/DataSyncLog.json" , rollingInterval: RollingInterval.Day , retainedFileCountLimit: 3)
                .MinimumLevel.Information()
                .CreateLogger();
            }
            else
            {
                // Send logs to Sentry
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft" , LogEventLevel.Warning)
                    .MinimumLevel.Override("System.Net.Http.HttpClient" , LogEventLevel.Warning)
                    .WriteTo.Console()
                    // Other overloads exist, for example, configure the SDK with only the DSN or no parameters at all.
                    .WriteTo.Sentry(o =>
                    {
                        // Debug and higher are stored as breadcrumbs (default os Information)
                        o.MinimumBreadcrumbLevel = LogEventLevel.Information;
                        // Error and higher is sent as event (default is Error)
                        o.MinimumEventLevel = LogEventLevel.Information;

                        o.AttachStacktrace = true;
                        // send PII like the username of the user logged in to the device
                        o.SendDefaultPii = true;
                        // Optional Serilog text formatter used to format LogEvent to string. If TextFormatter is set, FormatProvider is ignored.
                        o.TextFormatter = new MessageTemplateTextFormatter("DataSyncReport [{Timestamp:HH:mm:ss} {Level:u3}]- {Message:lj}");
                        // Other configuration

                        // Tells which project in Sentry to send events to:
                        o.Dsn = sentryOptions["Dsn"];
                        // When configuring for the first time, to see what the SDK is doing:
                        o.Debug = false;
                        // Set traces_sample_rate to 1.0 to capture 100% of transactions for performance monitoring.
                        // We recommend adjusting this value in production.
                        o.TracesSampleRate = 1.0;
                        // Enable Global Mode if running in a client app
                        o.IsGlobalModeEnabled = true;
                        o.Environment = isLocalDevelopment ? "NoSentryLog" : sentryOptions["Environment"];
                        o.DefaultTags.Add("FSComponent" , "DataSync Service");
                        o.BeforeSend = sentryEvent => FilterSentryEvents(sentryEvent , o);
                        o.Release = Version.DataSyncVersionNumber;

                    })
                    .CreateLogger();
            }
        }

        private SentryEvent? FilterSentryEvents(SentryEvent sentryEvent , SentrySerilogOptions o)
        {
            if (sentryEvent.Logger != null
              && (sentryEvent.Logger.Contains("Microsoft.EntityFrameworkCore.Model.Validation")
                    || sentryEvent.Logger.Contains("Microsoft.EntityFrameworkCore.Query")
                    || sentryEvent.Logger.Contains("Microsoft.EntityFrameworkCore.Infrastructure")
                    || sentryEvent.Logger.Contains("System.Net.Http.HttpClient")
                    )
              )
            {
                if (sentryEvent.Level == SentryLevel.Info)
                    return null; // Don't send these event to Sentry if level = Info (Warning or Error will goes to Sentry)
            }

            return sentryEvent;
        }

        public void LogInfo(string message , object data)
        {
            Log.Information(message , data);
        }


    }
}

