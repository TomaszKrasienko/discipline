using discipline.hangfire.browse_planned.DAL.Configuration;
using discipline.hangfire.shared.abstractions.Api;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.hangfire.browse_planned.Confguration;

public static class BrowsePlannedServiceConfigurationExtensions
{
    public static IServiceCollection SetBrowsePlanned(this IServiceCollection services)
        => services
            .AddDal()
            .AddScoped<IBrowsePlannedApi, BrowsePlannedApi>();
}