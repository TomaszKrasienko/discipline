using System.Reflection;
using FluentValidation;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class ValidationServicesConfigurationExtensions
{
    internal static IServiceCollection AddValidation(this IServiceCollection services, IList<Assembly> assemblies)
        => services
            .AddValidators(assemblies);
    
    private static IServiceCollection AddValidators(this IServiceCollection services, IList<Assembly> assemblies)
    {
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        services.AddValidatorsFromAssemblies(assemblies);
        return services;
    }
}