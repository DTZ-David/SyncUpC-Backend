using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SyncUpC.Infraestructure.Extensions;
using SyncUpC.Infraestructure.Extensions.Claims;
using SyncUpC.Infraestructure.Extensions.Feature;
using SyncUpC.Infraestructure.Extensions.JsonWebToken;
using SyncUpC.Infraestructure.Extensions.Mapper;
using SyncUpC.Infraestructure.Extensions.Mediador;
using SyncUpC.Infraestructure.Extensions.Middleware;
using SyncUpC.Infraestructure.Extensions.Persistence;
using SyncUpC.Infraestructure.Extensions.Services;
using SyncUpC.Infraestructure.Extensions.Swagger;
using SyncUpC.Infraestructure.Extensions.Validation;
namespace SyncUpC.Infraestructure;


public static class Startup
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediator()
            .AddFeature()
            .AddJsonWebToken(configuration)
            .AddApplicationServices(ProjectConstant.DomainProject)
            .AddSwagger()
            .AddMapper()
            .AddValidator()
            .AddPersistence(configuration)
            .AddClaims()
            .AddAuthorization()
            .AddCustomMiddleware();

    }

    public static void UseInfrastructure
    (
        this IApplicationBuilder builder,
        IWebHostEnvironment environment,
        IConfigurationBuilder configuration
    )
    {
        builder
            .UseSwagger(environment)
            .UseAuthentication()
            .UseAuthorization()
            .UseCustomMiddleware();
    }
}
