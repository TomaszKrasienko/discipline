using discipline.hangfire.activity_rule_modification.Events.External;
using discipline.libs.events.abstractions;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class AddActivityRulesServicesConfigurationExtensions
{
    public static IServiceCollection SetAddActivityRules(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddActivityRuleStrategyServices();

        services.AddConsumer<ActivityRuleModified>(sp => async (msg, ct, mt) =>
        {
            var handler = sp.GetRequiredService<IEventHandler<ActivityRuleModified>>();
            await handler.HandleAsync(msg, ct, mt);
        });
        
        return services;
    }
}