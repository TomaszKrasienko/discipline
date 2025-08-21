using discipline.centre.users.domain.Accounts.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.users.domain.Accounts.Services.Configuration;

internal static class AccountServicesConfigurationExtensions
{
    internal static IServiceCollection AddAccounts(this IServiceCollection services)
        => services.AddSingleton<IAccountService, AccountService>();
}