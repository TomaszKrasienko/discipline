using discipline.centre.users.domain.Accounts.Services;
using discipline.centre.users.domain.Accounts.Services.Abstractions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class AccountsDomainServicesConfigurationExtensions
{
    internal static IServiceCollection AddAccountServices(this IServiceCollection services)
        => services.AddSingleton<IAccountService, AccountService>();
}