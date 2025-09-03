using discipline.hangfire.activity_rules;
using discipline.hangfire.activity_rules.Clients.Configuration;
using discipline.hangfire.activity_rules.Events.External;
using discipline.hangfire.activity_rules.Strategies.Configuration;
using discipline.hangfire.infrastructure.Messaging.RabbitMq.Configuration;
using discipline.hangfire.shared.abstractions.Api;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class AddActivityRulesServicesConfigurationExtensions
{
    public static IServiceCollection SetAddActivityRules(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddActivityRuleClientService(configuration)
            .AddDal()
            .AddTransient<IActivityRulesApi, ActivityRulesApi>()
            .AddRabbitMqConsumer<ActivityRuleModified>()
            .AddActivityRuleStrategyServices();
}