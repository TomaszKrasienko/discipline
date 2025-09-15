using discipline.activity_scheduler.create_empty_daily_tracker.Api;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.activity_scheduler.create_empty_daily_tracker.Configuration;

public static class CreateEmptyUserDailyTrackerServicesConfigurationExtensions
{
    public static IServiceCollection SetCreateEmptyUserDailyTracker(this IServiceCollection services)
        => services
            .AddScoped<ICreateEmptyDailyTrackerApi, CreateEmptyDailyTrackerApi>();
}