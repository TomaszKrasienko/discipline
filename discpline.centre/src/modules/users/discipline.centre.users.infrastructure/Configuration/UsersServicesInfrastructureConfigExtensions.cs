using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class UsersServicesInfrastructureConfigExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string moduleNae,
        IConfiguration configuration)
        => services
            .AddPasswordsSecure()
            .AddDal(moduleNae)
            .AddAuth(configuration)
            .AddTokenStorage()
            .AddRefreshTokenStorage(configuration)
            .AddDataInitializers();
}