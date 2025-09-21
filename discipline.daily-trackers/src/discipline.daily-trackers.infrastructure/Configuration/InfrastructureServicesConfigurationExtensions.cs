using System.Reflection;
using discipline.daily_trackers.application.UserDailyTrackers.Commands.External;
using discipline.daily_trackers.infrastructure.Configuration.Options;
using discipline.libs.configuration;
using discipline.libs.cqrs.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class InfrastructureServicesConfigurationExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        List<Assembly> allAssemblies)
        => services
            .ValidateAndBind<AppOptions, AppOptionsValidator>(configuration)
            .AddCqrs(allAssemblies)
            .AddDal(configuration)
            .AddUiDocumentation()
            .AddRabbit(configuration);
    
    private static IServiceCollection AddUiDocumentation(this IServiceCollection services)
        => services.AddSwaggerGen(swagger =>
        {
            var appOptions = services.GetOptions<AppOptions>();
            swagger.EnableAnnotations();
            swagger.CustomSchemaIds(x => x.FullName);
            swagger.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = appOptions.Name,
                Version = "v1"
            });
        });

    private static IServiceCollection AddRabbit(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var appOptions = services.GetOptions<AppOptions>();
        services.AddRabbitMq(configuration, appOptions.Name!);

        services
            .AddConsumer<CreateEmptyUserDailyTrackerCommand>(sp => 
            {
                return (async (msg, ct, mt) =>
                {
                    using var scope = sp.CreateScope();
                    var dispatcher = scope.ServiceProvider.GetRequiredService<ICqrsDispatcher>();
                    await dispatcher.HandleAsync(msg.ToCommand(), ct);
                });
            })
            .AddConsumer<CreateActivitiesFromActivityRulesCommand>(sp =>
            {
                return async (msg, ct, mt) =>
                {
                    using var scope = sp.CreateScope();
                    var dispatcher = scope.ServiceProvider.GetRequiredService<ICqrsDispatcher>();
                    await dispatcher.HandleAsync(msg.ToCommand(), ct);
                };
            });
        
        return services;
    }
}