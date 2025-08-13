using discipline.centre.users.application.Accounts.Services;
using discipline.centre.users.infrastructure.TokenStorage;

// ReSharper disable once CheckNamespace
namespace  Microsoft.Extensions.DependencyInjection;

internal static class TokenStorageServicesConfigurationExtensions
{
    internal static IServiceCollection AddTokenStorage(this IServiceCollection services)
        => services
            .AddScoped<ITokenStorage, HttpContextTokenStorage>()
            .AddHttpContextAccessor();
}