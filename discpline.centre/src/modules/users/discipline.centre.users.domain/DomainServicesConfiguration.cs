using discipline.centre.users.domain.Accounts.Services.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DomainServicesConfiguration
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
        => services
            .AddPolicies()
            .AddAccounts();
}