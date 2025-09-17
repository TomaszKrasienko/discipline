using System.Reflection;
using discipline.libs.events;
using discipline.libs.events.abstractions;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class EventsServicesConfigurationExtensions
{
    public static IServiceCollection AddEvents(
        this IServiceCollection services,
        IEnumerable<Assembly> assemblies)
    {
        services
            .AddEventHandlers(assemblies)
            .AddSingleton<IEventDispatcher, EventDispatcher>();
        
        return services;
    }
    
    private static IServiceCollection AddEventHandlers(
        this IServiceCollection services,
        IEnumerable<Assembly> assemblies)
    {
        services.Scan(
            x => x.FromAssemblies(assemblies)
            .AddClasses(
                publicOnly:false,
                action: c => c.AssignableTo(typeof(IEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        return services;
    }
}