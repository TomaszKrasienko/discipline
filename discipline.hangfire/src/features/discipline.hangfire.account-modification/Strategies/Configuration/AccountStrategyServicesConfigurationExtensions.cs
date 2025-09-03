using discipline.hangfire.account_modification.Strategies;
using discipline.hangfire.account_modification.Strategies.Abstractions;


// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class AccountStrategyServicesConfigurationExtensions
{
    internal static IServiceCollection AddAccountStrategy(this IServiceCollection services)
        => services.AddScoped<IAccountHandlingStrategy, AccountRegisteredStrategy>();
}