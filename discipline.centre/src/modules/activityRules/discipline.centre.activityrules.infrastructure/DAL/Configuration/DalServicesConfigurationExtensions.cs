using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.infrastructure.DAL;
using discipline.centre.activityrules.infrastructure.DAL.Repositories;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using MongoDB.Driver;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services, string moduleName)
        => services
            .AddScoped<ActivityRulesMongoContext>(sp =>
            {
                var mongoClient = sp.GetRequiredService<IMongoClient>();
                var mongoCollectionNameConvention = sp.GetRequiredService<IMongoCollectionNameConvention>();
                return new ActivityRulesMongoContext(
                    mongoClient,
                    mongoCollectionNameConvention,
                    moduleName);
            })
            .AddRepositories<IReadActivityRuleRepository, IReadWriteActivityRuleRepository, MongoActivityRuleRepository>();
}