using System.Reflection;
using discipline.centre.shared.abstractions.Messaging;
using discipline.centre.shared.infrastructure.Events;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class EventsServicesConfigurationExtensions
{
    internal static IServiceCollection AddEvents(
        this IServiceCollection services,
        IConfiguration configuration,
        IEnumerable<Assembly> assemblies)
        => services
            .AddSingleton<IEventProcessor, EventProcessor>()
            .AddEvents(assemblies);
}