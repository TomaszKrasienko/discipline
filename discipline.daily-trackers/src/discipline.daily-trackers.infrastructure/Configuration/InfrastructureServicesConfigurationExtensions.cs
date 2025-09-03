using discipline.daily_trackers.infrastructure.Configuration.Options;
using discipline.libs.configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class InfrastructureServicesConfigurationExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .ValidateAndBind<AppOptions, AppOptionsValidator>(configuration)
            .AddDal(configuration)
            .AddUiDocumentation();
    
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
}