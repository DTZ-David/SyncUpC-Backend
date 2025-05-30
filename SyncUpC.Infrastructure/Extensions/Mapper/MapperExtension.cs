using Microsoft.Extensions.DependencyInjection;
using SyncUpC.Infraestructure.Extensions;
using System.Reflection;

namespace SyncUpC.Infraestructure.Extensions.Mapper;

public static class MapperExtension
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.Load(ProjectConstant.ApplicationProject));
        return services;
    }
}
