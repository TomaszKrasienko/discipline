using discipline.activity_scheduler.shared.infrastructure.Configuration.Options;
using discipline.activity_scheduler.shared.infrastructure.Logging.Configuration.Options;
using discipline.libs.configuration;
using Microsoft.Extensions.Configuration;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class LoggingServicesConfigurationExtensions
{
    internal static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddOptions(configuration)
            .AddDistributedTracing()
            .AddSerilog();

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services
            .ValidateAndBind<JaegerOptions, JaegerOptionsValidator>(configuration)
            .Configure<SeqOptions>(configuration.GetSection(nameof(SeqOptions)));
    
    private static IServiceCollection AddDistributedTracing(this IServiceCollection services)
    {
        var appOptions = services.GetOptions<AppOptions>();
        var jaegerOptions = services.GetOptions<JaegerOptions>();
        
        services.AddOpenTelemetry()
            .ConfigureResource(resource
                => resource.AddService(appOptions.Name!))
            .WithTracing(tracing 
                => tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(options => options.Endpoint = new Uri(jaegerOptions.Endpoint!)));
        
        return services;
    }
}