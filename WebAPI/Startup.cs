using ForecastingSystem.Application;
using ForecastingSystem.BackendAPI.DataSync;
using ForecastingSystem.BackendAPI.Middlewares;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.IoC;
using ForecastingSystem.DataSync.IoC;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForecastingSystem.BackendAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Setting Response Caching
            services.AddResponseCaching();

            // Setting In Memory Cache
            services.AddMemoryCache();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            RegisterAzureAD(services);

            // This is where we register the context
            services.AddDbContext<ForecastingSystemDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MyConnectionString")));

            //// Registering the Identity Infrastructure
            //services.AddIdentityInfrastructure();

            // Registering Inversion Of Control
            services.RegisterRepositories();
            services.RegisterServices();

            //// Registering the Swagger
            //services.AddSwaggerDocument(settings =>
            //{
            //    settings.Title = "Forcasting System Backend API";
            //    settings.Version = Version.VersionNumber;
            //});

            // Registering the HttpContext Accessor which we use for auditing in the LibraryDbContext
            services.AddHttpContextAccessor();

            // Registering the AutoMapper that mapps from entity to view model and vice versa
            services.AddApplicationLayer();

            // Sync data services
            services.AddTransientDataSyncServices(Configuration);

            //Hangfire
            services.AddTransient<AutomaticRetryAttribute>(x => new AutomaticRetryAttribute { Attempts = Configuration.GetValue<int>("DataSyncService:RetryAttempts") });
            services.AddHangfire((provider, configuration) =>
            {
                configuration.UseSqlServerStorage(Configuration.GetConnectionString("MyConnectionString"));
                configuration.UseFilter(provider.GetRequiredService<AutomaticRetryAttribute>());
            });
            services.AddHostedService<StartUpBackgroundJobService>();
            services.AddHangfireServer(option => new BackgroundJobServerOptions
            {
                CancellationCheckInterval = TimeSpan.FromSeconds(Configuration.GetValue<int>("DataSyncService:CancellationCheckInterval"))
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ConfigureAzureAD(app);

            app.UseHttpsRedirection();

            // Using the Response Caching
            app.UseResponseCaching();

            // Use Serilog Logger
            app.UseSerilogRequestLogging();

            // Error Logging middleware
            app.UseMiddleware<ErrorLoggingMiddleware>();


            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("MyPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (!env.IsDevelopment())
            {
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "wwwroot";
                });
            }
            app.UseHangfireDashboard();
        }

        private void ConfigureAzureAD(IApplicationBuilder app)
        {
            var swaggerUIClientId = Configuration["AzureAd:SwaggerUIClientId"];

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Forcasting System Backend API");
                c.OAuthClientId(swaggerUIClientId);
                c.OAuthUsePkce();
            }
            );
        }

        private void RegisterAzureAD(IServiceCollection services)
        {
            var tenantId = Configuration["AzureAd:TenantId"];
            var oauth2Url = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0";
            var authorizationUrl = $"{oauth2Url}/authorize";
            var tokenUrl = $"{oauth2Url}/token";

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Forcasting System Backend API", Version = Version.VersionNumber });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,

                    Flows = new OpenApiOAuthFlows()
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(authorizationUrl),
                            TokenUrl = new Uri(tokenUrl),
                            Scopes = new Dictionary<string, string>()
                                {
                                    { Configuration["AzureAd:Scopes"], "Read API"}
                                }
                        }
                    }
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                     {
                     new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                        {
                        Type = ReferenceType.SecurityScheme,
                        Id = "oauth2"
                        },
                                Scheme = "oauth2",
                                Name = "oauth2",
                                In = ParameterLocation.Header
                     },
                        new List<string>()
                     }
                });
            });
        }
    }
}
