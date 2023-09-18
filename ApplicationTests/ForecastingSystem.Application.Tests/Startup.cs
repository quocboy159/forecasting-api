using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.DependencyInjection;
using ForecastingSystem.Infrastructure.IoC;
using ForecastingSystem.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ForecastingSystem.Application.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
           services.RegisterServices();
            services.RegisterRepositories();
            services.AddDbContext<ForecastingSystemDbContext>(options =>
                options.UseSqlServer("MyConnectionString"));

        }
    }
}
