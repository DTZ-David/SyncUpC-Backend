using Microsoft.Extensions.DependencyInjection;
using SyncUpC.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Infraestructure.Extensions.Services;

public static class ServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, string projectAssemblyName)
    {
        var assembly = Assembly.Load(projectAssemblyName);

        var serviceImplementations = assembly.GetTypes().Where(type => type.IsClass && type.GetCustomAttribute<ApplicationServiceAttribute>() != null).ToList();

        foreach (var implementation in serviceImplementations)
        {
            var interfaceType = implementation.GetInterfaces().FirstOrDefault();
            if (interfaceType != null)
            {
                services.AddScoped(interfaceType, implementation);
            }
        }

        return services;
    }
}
