using Microsoft.Extensions.DependencyInjection;

namespace discipline.hangfire.browse_planned.DAL.Configuration;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services)
    {
        services.AddContext<BrowsePlannedDbContext>();
        return services;
    }
}