using discipline.centre.users.infrastructure.DataInitializers;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class SubscriptionServicesConfigurationExtensions
{
    internal static IServiceCollection AddDataInitializers(this IServiceCollection services)
        => services.AddHostedService<SubscriptionInitializer>();
}