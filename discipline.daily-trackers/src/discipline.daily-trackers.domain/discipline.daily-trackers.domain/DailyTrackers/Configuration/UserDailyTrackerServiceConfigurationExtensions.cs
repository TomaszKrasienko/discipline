using discipline.daily_trackers.domain.DailyTrackers.Services;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class UserDailyTrackerServiceConfigurationExtensions
{
    internal static IServiceCollection AddUserDailyTrackerDomain(this IServiceCollection services)
        => services
            .AddScoped<IUserDailyTrackerFactory, UserDailyTrackerFactory>();
}