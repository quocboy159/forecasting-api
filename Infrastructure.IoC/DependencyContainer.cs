using ForecastingSystem.Application.Interfaces;
using ForecastingSystem.Domain.Interfaces;
using ForecastingSystem.Infrastructure.Data.Context;
using ForecastingSystem.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ForecastingSystem.Infrastructure.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            var repositoryType = typeof(IPhaseResourceRepository);
            var classRepositoryType = typeof(PhaseResourceRepository);

            var allProviderTypes = Assembly.GetAssembly(repositoryType).GetTypes().Where(t => t.Namespace != null).ToList();
            IEnumerable<Type> iProviderTypes = allProviderTypes.Where(c => c.IsInterface && c.Name.IndexOf("Repository") > 0 && c.Name.IndexOf("ISync") < 0);
            IEnumerable<Type> classProviderTypes = Assembly.GetAssembly(classRepositoryType).GetTypes().Where(t => t.Namespace != null).ToList();
            foreach (var item in iProviderTypes)
            {
                var impl = classProviderTypes.FirstOrDefault(c => !c.IsAbstract && c.IsClass && item.Name.Substring(1) == c.Name);
                if (impl != null) services.AddScoped(item, impl);
            }
            //Remove Scoped DataSyncProcessRepository to add Transient DataSyncProcessRepository later
            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IDataSyncProcessRepository));
            services.Remove(serviceDescriptor);
        }

        public static void RegisterServices(this IServiceCollection services)
        {
            var serviceType = typeof(IPhaseResourceService);
            AddScopedByAssembly(services, serviceType);
        }

        public static void AddScopedByAssembly(this IServiceCollection services, Type type, Type classType = null)
        {
            var allProviderTypes = System.Reflection.Assembly.GetAssembly(type).GetTypes().Where(t => t.Namespace != null).ToList();
            IEnumerable<Type> classProviderTypes;
            if (classType != null)
            {
                classProviderTypes = System.Reflection.Assembly.GetAssembly(classType).GetTypes().Where(t => t.Namespace != null).ToList();
            }
            else {
                classProviderTypes = allProviderTypes;
            }
            foreach (var item in allProviderTypes.Where(t => t.IsInterface))
            {
                var impl = classProviderTypes.FirstOrDefault(c => c.IsClass && item.Name.Substring(1) == c.Name);
                if (impl != null) services.AddScoped(item, impl);
            }

        }
    }
}
