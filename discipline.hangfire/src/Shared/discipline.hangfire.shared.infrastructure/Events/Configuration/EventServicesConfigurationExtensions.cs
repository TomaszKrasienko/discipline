using discipline.hangfire.infrastructure.Events;
using discipline.hangfire.shared.abstractions.Events;
using System.Reflection;
using discipline.hangfire.infrastructure.Events.Abstractions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class EventServicesConfigurationExtensions
{
    internal static IServiceCollection AddEvents(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    { 
        services.Scan(s => s.FromAssemblies(assemblies)
            .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        services.AddSingleton<IEventDispatcher, EventDispatcher>();
        services.AddSingleton<IRouteRegister, RouteRegister>();
        
        return services;
    }
}