using discipline.centre.users.infrastructure.Subscriptions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class SubscriptionServicesConfigurationExtensions
{
    internal static IServiceCollection AddSubscriptions(this IServiceCollection services)
        => services.AddHostedService<SubscriptionInitializer>();
}