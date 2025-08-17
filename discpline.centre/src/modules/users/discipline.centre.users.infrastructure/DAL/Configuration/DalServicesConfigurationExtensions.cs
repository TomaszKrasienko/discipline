using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using discipline.centre.users.domain.Accounts.Repositories;
using discipline.centre.users.domain.Subscriptions.Repositories;
using discipline.centre.users.infrastructure.DAL.Users.Repositories;
using discipline.centre.users.domain.Users.Repositories;
using discipline.centre.users.infrastructure.DAL;
using discipline.centre.users.infrastructure.DAL.Accounts.Repositories;
using discipline.centre.users.infrastructure.DAL.Subscriptions.Repositories;
using MongoDB.Driver;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services, string moduleName)
        => services
            .AddScoped<UsersMongoContext>(sp =>
            {
                var mongoClient = sp.GetRequiredService<IMongoClient>();
                var mongoCollectionNameConvention = sp.GetRequiredService<IMongoCollectionNameConvention>();
                return new UsersMongoContext(
                    mongoClient,
                    mongoCollectionNameConvention,
                    moduleName);
            })
            .AddRepositories<IReadUserRepository, IReadWriteUserRepository, MongoUserRepository>()
            .AddRepositories<IReadAccountRepository, IReadWriteAccountRepository, MongoAccountRepository>()
            .AddRepositories<IReadSubscriptionRepository, IReadWriteSubscriptionRepository, MongoSubscriptionRepository>();
}