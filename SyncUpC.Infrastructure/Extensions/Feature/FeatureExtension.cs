using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SyncUpC.Infraestructure.Extensions.Feature;

public static class FeatureExtension
{
    public static IServiceCollection AddFeature(this IServiceCollection services)
    {
        services.AddLocalization();
        services.AddMvc();
         services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.DateFormatString = "dd/MM/yyyy HH:mm:ss";
            options.SerializerSettings.FloatParseHandling = FloatParseHandling.Double;
        });

    return services;
    }

    public static IApplicationBuilder UseFeature
    (
        this IApplicationBuilder app,
        IWebHostEnvironment environment,
        IConfigurationBuilder configuration
    )
    {
        app.UseHttpsRedirection();

        if (!environment.IsProduction())
        {
            configuration.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: false, reloadOnChange: true);
        }

        return app;
    }
}
