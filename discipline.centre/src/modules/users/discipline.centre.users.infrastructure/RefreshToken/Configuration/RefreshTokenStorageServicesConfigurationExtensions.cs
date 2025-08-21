using discipline.centre.users.application.Accounts.Services;
using discipline.centre.users.infrastructure.RefreshToken;
using discipline.centre.users.infrastructure.RefreshToken.Configuration;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class RefreshTokenStorageServicesConfigurationExtensions
{
    internal static IServiceCollection AddRefreshTokenStorage(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSingleton<IRefreshTokenManager, RefreshTokenSaver>()
            .ValidateAndBind<RefreshTokenOptions, RefreshTokenOptionsValidator>(configuration);
}