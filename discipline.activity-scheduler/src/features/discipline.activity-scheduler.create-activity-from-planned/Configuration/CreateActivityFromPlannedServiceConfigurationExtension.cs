using discipline.activity_scheduler.create_activity_from_planned.Apis;

// ReSharper disable once CheckNamespace
namespace  Microsoft.Extensions.DependencyInjection;

public static class CreateActivityFromPlannedServiceConfigurationExtension
{
    public static IServiceCollection SetCreateActivityFromPlanned(this IServiceCollection services)
        => services
            .AddTransient<ICreateActivitiesFromPlanned, CreateActivitiesFromPlanned>();
}