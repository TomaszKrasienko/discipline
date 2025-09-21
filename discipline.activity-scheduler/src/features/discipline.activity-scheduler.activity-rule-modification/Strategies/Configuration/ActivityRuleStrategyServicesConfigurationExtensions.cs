using discipline.activity_scheduler.activity_rule_modification.Strategies;
using discipline.activity_scheduler.activity_rule_modification.Strategies.Abstractions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class ActivityRuleStrategyServicesConfigurationExtensions
{
    internal static IServiceCollection AddActivityRuleStrategyServices(this IServiceCollection services)
        => services
            .AddScoped<IActivityRuleHandlingStrategy, ActivityRuleRegisteredStrategy>()
            .AddScoped<IActivityRuleHandlingStrategy, ActivityRuleChangedStrategy>()
            .AddScoped<IActivityRuleHandlingStrategy, ActivityRuleDeletedStrategy>();
}