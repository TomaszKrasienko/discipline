using discipline.hangfire.add_planned_tasks.DAL;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services)
        => services.AddContext<AddPlannedTaskDbContext>();
}