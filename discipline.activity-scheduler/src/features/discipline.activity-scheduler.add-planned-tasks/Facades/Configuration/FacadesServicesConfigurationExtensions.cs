using discipline.activity_scheduler.add_planned_tasks.Facades;

// ReSharper disable once CheckNamespace
namespace  Microsoft.Extensions.DependencyInjection;

internal static class FacadesServicesConfigurationExtensions
{
    internal static IServiceCollection AddFacades(this IServiceCollection services)
        => services.AddTransient<ICentreFacade, CentreFacade>();
}