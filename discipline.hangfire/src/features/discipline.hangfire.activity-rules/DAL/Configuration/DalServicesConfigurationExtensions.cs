using discipline.hangfire.activity_rules.DAL;
using discipline.hangfire.add_activity_rules.DAL;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services)
    {
        services.AddContext<ActivityRuleDbContext>();
        return services;
    }
}