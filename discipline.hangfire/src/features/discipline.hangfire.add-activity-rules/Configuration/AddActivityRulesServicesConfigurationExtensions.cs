using discipline.hangfire.add_activity_rules;
using discipline.hangfire.add_activity_rules.Clients.Configuration;
using discipline.hangfire.add_activity_rules.Events.External;
using discipline.hangfire.add_activity_rules.Facades;
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
            .AddTransient<ICentreFacade, CentreFacade>()
            .AddTransient<IAddActivityRulesApi,  AddActivityRulesApi>()
            .AddBrokerConsumer<ActivityRuleRegistered>();
}