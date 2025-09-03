using discipline.hangfire.activity_rules.Strategies.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.hangfire.activity_rules.Strategies.Configuration;

internal static class ActivityRuleStrategyServicesConfigurationExtensions
{
    internal static IServiceCollection AddActivityRuleStrategyServices(this IServiceCollection services)
        => services
            .AddScoped<IActivityRuleHandlingStrategy, ActivityRuleRegisteredStrategy>()
            .AddScoped<IActivityRuleHandlingStrategy, ActivityRuleChangedStrategy>()
            .AddScoped<IActivityRuleHandlingStrategy, ActivityRuleDeletedStrategy>();
}