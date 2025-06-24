using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using SyncUpC.Infraestructure.Pipeline;



namespace SyncUpC.Infraestructure.Extensions.Validation;
public static class ValidationExtension
{
    public static IServiceCollection AddValidator(this IServiceCollection services)
    {
        var validationAssembly = Assembly.Load(ProjectConstant.ApplicationProject);
        services.AddValidatorsFromAssembly(validationAssembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}
