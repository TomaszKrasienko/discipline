using discipline.hangfire.add_planned_tasks;
using discipline.hangfire.add_planned_tasks.Clients.Configuration;
using discipline.hangfire.shared.abstractions.Api;
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
            .AddTransient<IAddPlannedTasksApi, AddPlannedTasksApi>()
            .AddFacades()
            .AddDal()
            .AddCentreClient(configuration);
}