using SyncUpC.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.AddInfrastructure(configuration);

var app = builder.Build();
app.UseInfrastructure(app.Environment, configuration);
app.MapControllers();

await app.RunAsync();