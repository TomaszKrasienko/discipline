using System.Net.Http.Headers;
using discipline.activity_scheduler.add_planned_tasks.Clients.Configuration.Options;
using discipline.activity_scheduler.shared.abstractions.Auth;
using discipline.libs.configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace discipline.activity_scheduler.add_planned_tasks.Clients.Configuration;

internal static class CentreClientServicesConfigurationExtensions
{
    internal static IServiceCollection AddCentreClient(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddOptions(configuration)
            .AddHttpClient();

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services
            .ValidateAndBind<CentreClientOptions, CentreClientOptionsValidator>(configuration);
    
    private static IServiceCollection AddHttpClient(this IServiceCollection services)
    {
        services
            .AddRefitClient<ICentreActivityRuleClient>()
            .ConfigureHttpClient(x =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var tokenGenerator = scope.ServiceProvider.GetRequiredService<ICentreTokenGenerator>();
                var options = scope.ServiceProvider.GetRequiredService<IOptions<CentreClientOptions>>().Value;
                    
                var token = tokenGenerator.Get();
                
                x.BaseAddress = new Uri(options.Url);
                x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            });
        
        return services;
    }
}