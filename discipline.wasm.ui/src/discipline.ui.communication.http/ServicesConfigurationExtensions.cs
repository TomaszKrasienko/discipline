using discipline.ui.communication.http.ActivityRules;
using discipline.ui.communication.http.DailyTrackers;
using discipline.ui.communication.http.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace discipline.ui.communication.http;

public static class ServicesConfigurationExtensions
{
    public static IServiceCollection SetHttpCommunicationServices(this IServiceCollection services,
        IConfiguration configuration)
        => services
            .SetOptions(configuration)
            .SetHttpClients();

    private static IServiceCollection SetHttpClients(this IServiceCollection services)
    {
        var httpClientOptions = services.GetOptions<HttpClientOptions>().Value;

        services.AddSingleton<AuthorizeHandler>();
        
        services
            .AddRefitClient<IUserHttpClient>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(httpClientOptions.Url);
                c.Timeout = httpClientOptions.Timeout;
            })
            .AddHttpMessageHandler<AuthorizeHandler>();

        services
            .AddRefitClient<IActivityRulesHttpService>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(httpClientOptions.Url);
                c.Timeout = httpClientOptions.Timeout;
            })
            .AddHttpMessageHandler<AuthorizeHandler>();
        
        services
            .AddRefitClient<IDailyTrackerHttpService>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(httpClientOptions.Url);
                c.Timeout = httpClientOptions.Timeout;
            })
            .AddHttpMessageHandler<AuthorizeHandler>();

        return services;
    }
    
    private static IServiceCollection SetOptions(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<HttpClientOptions>(configuration.GetSection(nameof(HttpClientOptions)));

    private static IOptions<TOptions> GetOptions<TOptions>(this IServiceCollection services) where TOptions : class
    {
        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var options = scope.ServiceProvider.GetRequiredService<IOptions<TOptions>>();
        return options;
    }
}