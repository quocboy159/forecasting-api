using ForecastingSystem.Domain.Common;
using ForecastingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForecastingSystem.Infrastructure.Data.Context
{
    public class ForecastingSystemDbContextSeed
    {
        public static async Task SeedAsync(ForecastingSystemDbContext dbContext , ILoggerFactory loggerFactory , int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                // NOTE : Only run this if using a real database
                dbContext.Database.Migrate();
                dbContext.Database.EnsureCreated();

                await SeedRatesAsync(dbContext);
                await SeedDefaultResourcePlaceHolderAsync(dbContext);
            }
            catch (Exception exception)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<ForecastingSystemDbContextSeed>();
                    log.LogError(exception.Message);
                    await SeedAsync(dbContext , loggerFactory , retryForAvailability);
                }
                throw;
            }
        }

        private static async Task SeedRatesAsync(ForecastingSystemDbContext dbContext)
        {
            if (dbContext.Rates.Any())
                return;
            var rates = new List<Rate>() {
                new Rate { RateName = "NZ Project Manager", HourlyRate = 195, UpdatedDateTime = DateTime.UtcNow, IsActive = true },
                new Rate { RateName = "NZ Architect", HourlyRate = 195, UpdatedDateTime = DateTime.UtcNow, IsActive = true },
                new Rate { RateName = "NZ Tech Lead", HourlyRate = 170, UpdatedDateTime = DateTime.UtcNow, IsActive = true },
                new Rate { RateName = "NZ Developer", HourlyRate = 165, UpdatedDateTime = DateTime.UtcNow, IsActive = true },
                new Rate { RateName = "NZ Business Analyst", HourlyRate = 165, UpdatedDateTime = DateTime.UtcNow, IsActive = true },
                new Rate { RateName = "VN Developer", HourlyRate = 85, UpdatedDateTime = DateTime.UtcNow, IsActive = true },
                new Rate { RateName = "VN Tester", HourlyRate = 85, UpdatedDateTime = DateTime.UtcNow, IsActive = true },
                new Rate { RateName = "VN Tech Lead", HourlyRate = 90, UpdatedDateTime = DateTime.UtcNow, IsActive = true }
            };

            dbContext.Rates.AddRange(rates);
            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedDefaultResourcePlaceHolderAsync(ForecastingSystemDbContext dbContext)
        {
            if (dbContext.DefaultResourcePlaceHolders.Any())
                return;
            var resources = new List<DefaultResourcePlaceHolder>() {
                new DefaultResourcePlaceHolder { Name = "NZ Project Manager (Place holder)", Country = "New Zealand"},
                new DefaultResourcePlaceHolder { Name = "NZ Architect (Place holder)", Country = "New Zealand" },
                new DefaultResourcePlaceHolder { Name = "NZ Tech Lead (Place holder)", Country = "New Zealand" },
                new DefaultResourcePlaceHolder { Name = "NZ Developer (Place holder)", Country = "New Zealand" },
                new DefaultResourcePlaceHolder { Name = "NZ Business Analyst (Place holder)", Country = "New Zealand" },
                new DefaultResourcePlaceHolder { Name = "VN Developer (Place holder)", Country = "Viet Nam" },
                new DefaultResourcePlaceHolder { Name = "VN Tester (Place holder)", Country = "Viet Nam" },
                new DefaultResourcePlaceHolder { Name = "VN Tech Lead (Place holder)", Country = "Viet Nam" }
            };

            dbContext.DefaultResourcePlaceHolders.AddRange(resources);
            await dbContext.SaveChangesAsync();
        }
    }


}
