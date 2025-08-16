using discipline.centre.users.domain.Subscriptions.Policies;
using discipline.centre.users.domain.Subscriptions.Policies.Abstractions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class SubscriptionPoliciesServicesConfigurationExtensions
{
    internal static IServiceCollection AddPolicies(this IServiceCollection services)
        => services
            .AddSingleton<ISubscriptionPolicy, StandardSubscriptionPolicy>()
            .AddSingleton<ISubscriptionPolicy, PremiumSubscriptionPolicy>()
            .AddSingleton<ISubscriptionPolicy, AdminSubscriptionPolicy>();
}