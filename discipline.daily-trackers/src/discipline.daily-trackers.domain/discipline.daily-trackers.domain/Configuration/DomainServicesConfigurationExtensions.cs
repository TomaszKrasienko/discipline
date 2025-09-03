
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DomainServicesConfigurationExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
        => services.AddUserDailyTrackerDomain();
}