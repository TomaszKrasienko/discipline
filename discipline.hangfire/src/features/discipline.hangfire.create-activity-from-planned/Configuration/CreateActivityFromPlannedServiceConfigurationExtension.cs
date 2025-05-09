using discipline.hangfire.create_activity_from_planned.Publishers.Configuration;
using discipline.hangfire.shared.abstractions.Api;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.hangfire.create_activity_from_planned.Configuration;

public static class CreateActivityFromPlannedServiceConfigurationExtension
{
    public static IServiceCollection SetCreateActivityFromPlanned(this IServiceCollection services)
        => services
            .AddTransient<ICreateActivityFromPlannedApi, CreateActivityFromPlannedApi>()
            .AddBroker();
}