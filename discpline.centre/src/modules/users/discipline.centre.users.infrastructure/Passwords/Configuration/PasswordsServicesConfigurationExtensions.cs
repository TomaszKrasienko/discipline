using discipline.centre.users.domain.Accounts.Services.Abstractions;
using discipline.centre.users.domain.Users;
using discipline.centre.users.infrastructure.Passwords;
using Microsoft.AspNetCore.Identity;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class PasswordsServicesConfigurationExtensions
{
    //TODO: Name change
    internal static IServiceCollection AddPasswordsSecure(this IServiceCollection services)
        => services
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddSingleton<IPasswordManager, PasswordManager>();
}