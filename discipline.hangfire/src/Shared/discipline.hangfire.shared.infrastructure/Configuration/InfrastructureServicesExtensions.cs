using System.Reflection;
using discipline.hangfire.infrastructure.Configuration.Options;
using discipline.hangfire.infrastructure.Logging.Configuration;
using discipline.hangfire.infrastructure.Messaging.Configuration;
using discipline.hangfire.infrastructure.Postgres.Configuration;
using discipline.hangfire.infrastructure.Serializer.Configuration;
using discipline.hangfire.infrastructure.Time.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace discipline.hangfire.infrastructure.Configuration;

/// <summary>
/// Extensions to configure infrastructure-related services
/// for the application
/// </summary>
public static class InfrastructureServicesExtensions
{
    /// <summary>
    /// Configures infrastructure services required by the project
    /// </summary>
    /// <param name="services">The Dependency Injection (DI) container for registering services</param>
    /// <param name="configuration">Abstraction providing access to the application's configuration settings</param>
    /// <param name="assemblies">List of all assemblies</param>
    /// <returns>The updated DI services container as instance of <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration,
        IEnumerable<Assembly> assemblies)
        => services
            .AddOptions(configuration)
            .AddAuth(configuration)
            .AddClock()
            .AddPostgres(configuration)
            .AddEvents(assemblies)
            .AddSerializer()
            .AddRedisBroker(configuration)
            .AddLogging(configuration)
            .AddSingleton(TimeProvider.System)
            .AddMessaging(configuration);

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services.ValidateAndBind<AppOptions, AppOptionsValidator>(configuration);
    
    /// <summary>
    /// Extension for <see cref="IServiceCollection"/> that validates options at startup and registers them in dependency injection container. 
    /// </summary>
    /// <param name="services">The Dependency Injection container, see <see cref="IServiceCollection"/> for more details</param>
    /// <param name="configuration">The Configuration source, typically loaded from secrets or appsettings. See <see cref="IConfiguration"/> for more details.</param>
    /// <typeparam name="TOptions">The type of options being registered.</typeparam>
    /// <typeparam name="TOptionsValidator">Set of validation rules for <see cref="TOptions"/>.</typeparam>
    /// <returns>Extended <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection ValidateAndBind<TOptions, TOptionsValidator>(this IServiceCollection services,
        IConfiguration configuration) where TOptions : class where TOptionsValidator : class, IValidateOptions<TOptions>
    {
        services
            .AddOptions<TOptions>()
            .Bind(configuration.GetSection(typeof(TOptions).Name))
            .ValidateOnStart();
        services.AddSingleton<IValidateOptions<TOptions>, TOptionsValidator>();
        
        return services;
    }
    
    internal static TOptions GetOptions<TOptions>(this IServiceCollection services) where TOptions : class
    {
        var sp = services.BuildServiceProvider();
        return sp.GetRequiredService<IOptions<TOptions>>().Value;
    }
}