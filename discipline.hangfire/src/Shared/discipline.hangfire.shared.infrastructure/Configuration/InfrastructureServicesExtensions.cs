using System.Reflection;
using discipline.hangfire.infrastructure.Configuration.Options;
using discipline.hangfire.infrastructure.DAL.Configuration;
using discipline.hangfire.infrastructure.Messaging.Configuration;
using discipline.hangfire.infrastructure.Time.Configuration;
using discipline.libs.configuration;
using discipline.libs.events.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace discipline.hangfire.infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration,
        IEnumerable<Assembly> assemblies)
        => services
            .ValidateAndBind<AppOptions, AppOptionsValidator>(configuration)
            .AddAuth(configuration)
            .AddClock()
            .AddEvents(assemblies)
            .AddSerialization()
            .AddLogging(configuration)
            .AddSingleton(TimeProvider.System)
            .AddMessaging(configuration)
            .AddDal(configuration);
}