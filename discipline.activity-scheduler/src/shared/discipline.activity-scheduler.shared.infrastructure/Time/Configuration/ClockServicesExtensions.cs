using discipline.activity_scheduler.shared.abstractions.Time;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.activity_scheduler.shared.infrastructure.Time.Configuration;

internal static class ClockServicesExtensions
{
    internal static IServiceCollection AddClock(this IServiceCollection services)
        => services.AddSingleton<IClock, Clock>();
}