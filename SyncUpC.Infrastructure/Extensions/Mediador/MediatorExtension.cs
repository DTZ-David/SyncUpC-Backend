using Microsoft.Extensions.DependencyInjection;
using SyncUpC.Infraestructure.Extensions;
using System.Reflection;

namespace SyncUpC.Infraestructure.Extensions.Mediador;

public static class MediatorExtension
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        var assembly = Assembly.Load(ProjectConstant.ApplicationProject);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        return services;
    }
}
