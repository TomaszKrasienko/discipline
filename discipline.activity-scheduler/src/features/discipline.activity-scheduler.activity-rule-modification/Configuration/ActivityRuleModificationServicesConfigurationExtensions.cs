using discipline.activity_scheduler.activity_rule_modification.Events.External;
using discipline.activity_scheduler.activity_rule_modification.Events.External.Handlers;
using discipline.libs.events.abstractions;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ActivityRuleModificationServicesConfigurationExtensions
{
    public static IServiceCollection SetAddActivityRules(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddActivityRuleStrategyServices()
            .AddScoped<IEventHandler<ActivityRuleModified>, ActivityRuleModifiedHandler>();

        services.AddConsumer<ActivityRuleModified>(sp =>
        {
            return (async (msg, ct, mt) =>
            {
                using var scope = sp.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IEventHandler<ActivityRuleModified>>();
                await handler.HandleAsync(msg, ct, mt);
            });
        });
        
        return services;
    }
}