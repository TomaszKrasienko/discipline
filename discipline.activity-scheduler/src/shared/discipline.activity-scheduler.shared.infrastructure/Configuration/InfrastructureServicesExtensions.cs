using System.Reflection;
using discipline.activity_scheduler.shared.infrastructure.Configuration.Options;
using discipline.activity_scheduler.shared.infrastructure.DAL.Configuration;
using discipline.activity_scheduler.shared.infrastructure.Messaging.Configuration;
using discipline.activity_scheduler.shared.infrastructure.Time.Configuration;
using discipline.libs.configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.activity_scheduler.shared.infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IEnumerable<Assembly> assemblies)
        => services
            .ValidateAndBind<AppOptions, AppOptionsValidator>(configuration)
            .AddAuth(configuration)
            .AddClock()
            .AddSerialization()
            .AddLogging(configuration)
            .AddSingleton(TimeProvider.System)
            .AddMessaging(configuration)
            .AddDal(configuration);
}