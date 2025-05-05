using discipline.ui.infrastructure.Users.SignIn;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class UsersServicesConfigurationExtensions
{
    internal static IServiceCollection SetUsersServices(this IServiceCollection services)
        => services
            .AddTransient<ISignInFacade, SignInFacade>();
}