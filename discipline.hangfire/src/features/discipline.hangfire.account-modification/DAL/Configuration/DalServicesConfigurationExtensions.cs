using discipline.hangfire.account_modification.DAL;
using discipline.hangfire.account_modification.DAL.Repositories;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services)
    {
        services.AddContext<AccountDbContext>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        return services;
    }
}