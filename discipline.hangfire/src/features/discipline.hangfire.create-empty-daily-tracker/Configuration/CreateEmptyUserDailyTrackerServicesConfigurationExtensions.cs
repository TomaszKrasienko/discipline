using discipline.hangfire.create_empty_daily_tracker.Api;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.hangfire.create_empty_daily_tracker.Configuration;

public static class CreateEmptyUserDailyTrackerServicesConfigurationExtensions
{
    public static IServiceCollection SetCreateEmptyUserDailyTracker(this IServiceCollection services)
        => services
            .AddScoped<ICreateEmptyDailyTrackerApi, CreateEmptyDailyTrackerApi>();
}