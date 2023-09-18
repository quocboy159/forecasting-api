using AutoMapper;
using ForecastingSystem.DataSyncServices;
using ForecastingSystem.DataSyncServices.DataAccessServices;
using ForecastingSystem.DataSyncServices.DataAccessServices.Interfaces;
using ForecastingSystem.DataSyncServices.Helpers;
using ForecastingSystem.DataSyncServices.Interfaces;
using ForecastingSystem.DataSyncServices.Outbound;
using ForecastingSystem.DataSyncServices.Outbound.Interfaces;
using ForecastingSystem.Domain;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Domain.Interfaces.SyncInterfaces;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories;
using ForecastingSystem.Infrastructure.Data.SyncRepositories;
using ForecastingSystem.Infrastructure.SyncDataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Reflection;

namespace ForecastingSystem.DataSync.IoC
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDataSyncServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<SyncForecastingSystemDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("MyConnectionString")));

            services.AddScoped<ISyncPublicHolidayRepository , SyncPublicHolidayRepository>();
            services.AddScoped<ISyncClientRepository , SyncClientRepository>();
            services.AddScoped<ISyncProjectRepository , SyncProjectRepository>();
            services.AddScoped<ISyncProjectPhaseRepository , SyncProjectPhaseRepository>();            
            services.AddScoped<ISyncProjectRateRepository , SyncProjectRateRepository>();
            services.AddScoped<ISyncProjectRateHistoryRepository , SyncProjectRateHistoryRepository>();
            services.AddScoped<ISyncProjectEmployeeManagerRepository, SyncProjectEmployeeManagerRepository>();

            services.AddScoped<IClientService , ClientService>();
            services.AddScoped<IProjectService , ProjectService>();
            services.AddScoped<IPhaseService , PhaseService>();
            services.AddScoped<IProjectRateService , ProjectRateService>();
            services.AddScoped<IProjectRateHistoryService , ProjectRateHistoryService>();
            
            services.AddScoped<IEmployeeService , EmployeeService>();
            services.AddScoped<ISyncSkillsetCategoryRepository , SyncSkillsetCategoryRepository>();
            services.AddScoped<ISyncEmployeeSkillsetRepository , SyncEmployeeSkillsetRepository>();
            services.AddScoped<ISyncSkillsetRepository , SyncSkillsetRepository>();
            services.AddScoped<ISyncSalaryRepository , SyncSalaryRepository>();
            services.AddScoped<ISyncEmployeeRepository , SyncEmployeeRepository>();
            services.AddScoped<ISyncUserIdLookupRepository , SyncUserIdLookupRepository>();
            services.AddScoped<IEmployeePersistentService , EmployeePersistentService>();
            services.AddScoped<ISkillsetPersistentService , SkillsetPersistentService>();
            services.AddScoped<IDataSyncProcessRepository , DataSyncProcessRepository>();
            services.AddScoped<ITimesheetEntryService , TimesheetEntryService>();
            services.AddScoped<ISyncEmployeeTimesheetEntryRepository , SyncEmployeeTimesheetEntryRepository>();
            services.AddScoped<DatetimeHelper , DatetimeHelper>();
            services.AddScoped<IForecastingSystemEncryptor , ForecastingSystemEncryptor>();            
            services.AddSingleton<IForecastingSystemEncryptor>(
                new ForecastingSystemEncryptor()
                .UseEncryptionKey(configuration.GetValue<string>("EncryptionDecryptionKey")));
            // Setting AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IDataSyncStartup , DataSyncStartup>();
            services.AddScoped<IDataSyncServiceLogger , DataSyncServiceLogger>();
            services.AddScoped<IBambooHRConnectionFactory , BambooHRConnectionFactory>();
            services.AddScoped<ITimesheetConnectionFactory , TimesheetConnectionFactory>();

            services.AddScoped<IDataSync , DataSyncService>();
            
            services.AddScoped<ISyncJob , TimesheetClientSyncJob>();
            services.AddScoped<ISyncJob , TimesheetProjectSyncJob>();
            services.AddScoped<ISyncJob , LeaveSystemSyncJob>();
            services.AddScoped<ISyncJob , BambooHREmployeeDetailsSyncJob>();
            services.AddScoped<ISyncJob , BambooHRMetadataSyncJob>();
            services.AddScoped<ISyncJob , TimesheetEntrySyncJob>(); 
            services.AddScoped<ISyncJob , TimesheetPhaseSyncJob>(); 
            services.AddScoped<ISyncJob , TimesheetProjectRateSyncJob>(); 
            services.AddScoped<ISyncJob , TimesheetProjectRateHistorySyncJob>(); 

            services.AddScoped<IDataSyncProcessService , DataSyncProcessService>();


            services.AddHttpClient("BambooHrApi")
                .AddPolicyHandler(Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).RetryAsync(3));

            services.AddHttpClient("TimesheetAPI")
                .AddPolicyHandler(Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).RetryAsync(3));

            return services;
        }

        public static IServiceCollection AddTransientDataSyncServices(this IServiceCollection services , IConfiguration configuration)
        {
            // Setting AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransientRepositories(configuration);
            services.AddTransientServices();
            services.AddTransient<DatetimeHelper , DatetimeHelper>();
            services.AddTransient<IDataSync , DataSyncService>();
            services.AddTransient<IDataSyncServiceLogger , DataSyncServiceLogger>();
            services.AddTransient<IBambooHRConnectionFactory , BambooHRConnectionFactory>();
            services.AddTransient<ITimesheetConnectionFactory , TimesheetConnectionFactory>();
            services.AddTransientSyncJobs();
            services.AddTransient<IDataSyncStartup , DataSyncStartup>();
            services.AddSingleton<IForecastingSystemEncryptor>(
              new ForecastingSystemEncryptor()
              .UseEncryptionKey(configuration.GetValue<string>("EncryptionDecryptionKey")));

            services.AddHttpClient("BambooHrApi")
                .AddPolicyHandler(Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).RetryAsync(3));

            services.AddHttpClient("TimesheetAPI")
                .AddPolicyHandler(Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).RetryAsync(3));

            return services;
        }

        private static void AddTransientSyncJobs(this IServiceCollection services)
        {
            var type = typeof(ISyncJob);
            var allProviderTypes = Assembly.GetAssembly(type).GetTypes().Where(t => t.Namespace != null).ToList();
            IEnumerable<Type> classProviderTypes = allProviderTypes.Where(c => !c.IsAbstract && c.IsClass && c.Name.IndexOf(type.Name.Substring(1)) > 0);
            foreach (var item in classProviderTypes)
            {
                services.AddTransient(type , item);
            }
        }

        private static void AddTransientRepositories(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<SyncForecastingSystemDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MyConnectionString")) , ServiceLifetime.Transient , ServiceLifetime.Transient);

            var type = typeof(ISyncClientRepository);
            var classRepositoryType = typeof(SyncClientRepository);
            var allProviderTypes = Assembly.GetAssembly(type).GetTypes().Where(t => t.Namespace != null).ToList();
            IEnumerable<Type> iProviderTypes = allProviderTypes.Where(c => c.IsInterface && c.Name.IndexOf("Repository") > 0 && c.Name.IndexOf("ISync") == 0);
            IEnumerable<Type> classProviderTypes = Assembly.GetAssembly(classRepositoryType).GetTypes().Where(t => t.Namespace != null).ToList();
            foreach (var item in iProviderTypes)
            {
                var impl = classProviderTypes.FirstOrDefault(c => !c.IsAbstract && c.IsClass && item.Name.Substring(1) == c.Name);
                if (impl != null) services.AddTransient(item , impl);
            }
            //add Transient DataSyncProcessRepository
            services.AddTransient<IDataSyncProcessRepository , DataSyncProcessRepository>();
           // services.AddTransient<IUserIdLookupRepository , UserIdLookupRepository>();
        }

        private static void AddTransientServices(this IServiceCollection services)
        {
            var type = typeof(IClientService);
            var allProviderTypes = Assembly.GetAssembly(type).GetTypes().Where(t => t.Namespace != null).ToList();
            IEnumerable<Type> iProviderTypes = allProviderTypes.Where(c => c.IsInterface && c.Name.IndexOf("Service") > 0);
            foreach (var item in iProviderTypes)
            {
                var impl = allProviderTypes.FirstOrDefault(c => !c.IsAbstract && c.IsClass && item.Name.Substring(1) == c.Name);
                if (impl != null) services.AddTransient(item , impl);
            }
        }
    }
}
