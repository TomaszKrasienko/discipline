using discipline.hangfire.add_planned_tasks.Api;
using discipline.hangfire.add_planned_tasks.Clients.Configuration;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions method for setting services for 'add planned tasks' use case
/// </summary>
public static class AddPlannedTasksServicesConfigurationExtensions
{
    public static IServiceCollection SetAddPlannedTasks(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddScoped<IAddPlannedTasksApi, AddPlannedTasksApi>()
            .AddFacades()
            .AddCentreClient(configuration);
}