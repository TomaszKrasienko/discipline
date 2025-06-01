using discipline.ui.infrastructure.ActivityRules.Facades;
using discipline.ui.infrastructure.DailyTrackers.DailyTrackers;
using discipline.ui.infrastructure.Facades.ActivityRules.Facades;
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
            .AddTransient<ICreateActivityRuleStageFacade, CreateActivityRuleStageFacade>()
            .AddTransient<IGetModesFacade, GetModesFacade>()
            .AddTransient<IActivityRuleClientFacade, ActivityRuleClientFacade>()
            .AddTransient<IDailyTrackerClientFacade, DailyTrackerClientFacade>()
            .SetUsersServices()
            .SetDailyTrackersServices()
            .SetStorageServices()
            .SetAuthServices();
}