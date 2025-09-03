using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace discipline.libs.configuration;

public static class DisciplineServicesConfigurationExtensions
{
    public static IServiceCollection ValidateAndBind<TOptions, TOptionsValidator>(
        this IServiceCollection services,
        IConfiguration configuration) where TOptions : class where TOptionsValidator : class, IValidateOptions<TOptions>
    {
        services
            .AddOptions<TOptions>()
            .Bind(configuration.GetSection(typeof(TOptions).Name))
            .ValidateOnStart();
        services.AddSingleton<IValidateOptions<TOptions>, TOptionsValidator>();
        
        return services;
    }
    
    public static TOptions GetOptions<TOptions>(this IServiceCollection services) where TOptions : class
    {
        var sp = services.BuildServiceProvider();
        return sp.GetRequiredService<IOptions<TOptions>>().Value;
    }
}