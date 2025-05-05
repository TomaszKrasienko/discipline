using discipline.ui.communication.http.Auth;
using discipline.ui.infrastructure.Auth.State;
using discipline.ui.infrastructure.Auth.Tokens;
using Microsoft.AspNetCore.Components.Authorization;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class AuthServicesConfigurationExtensions
{
    internal static IServiceCollection SetAuthServices(this IServiceCollection services)
        => services
            .AddSingleton<ITokenHandler, TokenHandler>()
            .AddSingleton<CustomAuthenticationStateProvider>()
            .AddSingleton<AuthenticationStateProvider>(sp 
                => sp.GetRequiredService<CustomAuthenticationStateProvider>());
}