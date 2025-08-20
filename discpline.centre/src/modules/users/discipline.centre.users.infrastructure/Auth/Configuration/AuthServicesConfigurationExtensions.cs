using discipline.centre.users.application.Accounts.Services;
using discipline.centre.users.infrastructure.Auth;
using discipline.centre.users.infrastructure.Auth.Configuration.Options;
using discipline.centre.users.infrastructure.Auth.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class AuthServicesConfigurationExtensions
{
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSingleton<IAuthenticator, JwtAuthenticator>()
            .AddSingleton<IAuthorizationHandler, AccountSubscriptionAuthorizationHandler>()
            .ValidateAndBind<JwtOptions, JwtOptionsValidator>(configuration);
}