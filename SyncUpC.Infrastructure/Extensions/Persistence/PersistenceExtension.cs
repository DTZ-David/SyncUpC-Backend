using SyncUpC.Domain.Ports;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SyncUpC.Infraestructure.Adapters;
using SyncUpC.Infraestructure.Context;


namespace SyncUpC.Infraestructure.Extensions.Persistence;


public static class PersistenceExtension
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Obtener la configuración desde variables de entorno o appsettings.json
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
            ?? configuration["DatabaseSettings:ConnectionString"]
            ?? throw new Exception("DB_CONNECTION_STRING no está configurado.");

        var databaseName = Environment.GetEnvironmentVariable("DB_NAME")
            ?? configuration["DatabaseSettings:DatabaseName"]
            ?? throw new Exception("DB_NAME no está configurado.");

        services.AddSingleton(typeof(MongoContext<>));

        services.AddSingleton(sp =>
        {
            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        });

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
