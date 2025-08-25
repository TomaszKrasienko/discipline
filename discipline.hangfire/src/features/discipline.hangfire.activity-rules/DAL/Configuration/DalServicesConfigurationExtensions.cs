using discipline.hangfire.activity_rules.DAL;
using discipline.hangfire.activity_rules.DAL.Repositories;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services)
    {
        services.AddContext<ActivityRuleDbContext>();
        services.AddScoped<IActivityRuleRepository, ActivityRuleRepository>();
        return services;
    }
}