using discipline.hangfire.infrastructure.Auth;
using discipline.hangfire.infrastructure.Auth.Configuration;
using discipline.hangfire.infrastructure.Configuration;
using discipline.hangfire.shared.abstractions.Auth;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class AuthServicesConfiguration
{
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddOptions(configuration)
            .AddServices();

    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        => services
            .ValidateAndBind<JwtOptions, JwtOptionsValidator>(configuration);

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddSingleton<ICentreTokenGenerator, CentreJwtTokenGenerator>();
}