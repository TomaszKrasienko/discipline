using discipline.ui.infrastructure.ActivityRules.Facades;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.ui.infrastructure;

public static class InfrastructureServicesConfiguration
{
    public static IServiceCollection SetInfrastructureServices(this IServiceCollection services)
        => services
            .AddTransient<IActivityRulesBrowseFacade, ActivityRulesBrowseFacade>()
            .AddTransient<IDeleteActivityRuleFacade, DeleteActivityRuleFacade>()
            .AddTransient<ICreateActivityRuleFacade, CreateActivityRuleFacade>()
            .AddTransient<IUpdateActivityRuleFacade, UpdateActivityRuleFacade>()
            .AddTransient<IGetModesFacade, GetModesFacade>()
            .SetUsersServices()
            .SetDailyTrackersServices()
            .SetStorageServices()
            .SetAuthServices();
}