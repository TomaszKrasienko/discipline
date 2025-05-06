using System.Net.Http.Headers;
using discipline.hangfire.infrastructure.Configuration;
using discipline.hangfire.shared.abstractions.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace discipline.hangfire.add_activity_rules.Clients.Configuration;

internal static class ActivityRuleClientServiceConfiguration
{
    internal static IServiceCollection AddActivityRuleClientService(this IServiceCollection services,
        IConfiguration configuration)
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