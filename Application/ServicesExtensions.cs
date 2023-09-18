using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ForecastingSystem.Application
{
    public static class ServicesExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            // Setting AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // we can add other services here
        }
    }
}
