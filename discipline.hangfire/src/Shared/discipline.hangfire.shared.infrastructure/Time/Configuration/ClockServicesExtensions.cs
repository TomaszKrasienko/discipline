using discipline.hangfire.shared.abstractions.Time;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.hangfire.infrastructure.Time.Configuration;

internal static class ClockServicesExtensions
{
    internal static IServiceCollection AddClock(this IServiceCollection services)
        => services.AddSingleton<IClock, Clock>();
}