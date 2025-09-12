using System.Reflection;
using discipline.libs.events.abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.libs.events.Configuration;

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
        //TODO: This process does not see internal event handlers. To fix.
        services.Scan(x => x.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        return services;
    }
}