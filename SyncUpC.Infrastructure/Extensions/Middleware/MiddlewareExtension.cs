using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SyncUpC.Infraestructure.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncUpC.Infraestructure.Extensions.Middleware;

public static class MiddlewareExtension
{
    public static IServiceCollection AddCustomMiddleware(this IServiceCollection services)
    {
        services.AddTransient<GlobalExceptionHandler>();
        //services.AddTransient<SecurityHeader>();

        return services;
    }

    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionHandler>();
        //app.UseMiddleware<SecurityHeader>();

        return app;
    }
}
