using discipline.centre.shared.abstractions.Messaging;
using discipline.centre.shared.infrastructure.Commands;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class CommandsServicesConfigurationExtensions
{
    internal static IServiceCollection AddCommandsPublishing(this IServiceCollection services)
        => services
            .AddScoped<ICommandProcessor, CommandProcessor>();
}