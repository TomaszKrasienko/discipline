using discipline.daily_trackers.domain.DailyTrackers.Repositories;
using discipline.daily_trackers.infrastructure.Configuration.Options;
using discipline.daily_trackers.infrastructure.DAL;
using discipline.daily_trackers.infrastructure.DAL.Repositories;
using discipline.libs.mongo_db.Abstractions;
using discipline.libs.mongo_db.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddMongoDb(configuration)
            .AddScoped<DailyTrackerMongoContext>(sp =>
            {
                var mongoClient = sp.GetRequiredService<IMongoClient>();
                var convention = sp.GetRequiredService<IMongoCollectionNameConvention>();
                var appOptions = sp.GetRequiredService<IOptions<AppOptions>>();
                
                return new DailyTrackerMongoContext(
                    mongoClient,
                    convention,
                    appOptions.Value.Name!);
            })
            .AddScoped<IReadUserDailyTrackerRepository, MongoUserDailyTrackerRepository>()
            .AddScoped<IReadWriteUserDailyTrackerRepository, MongoUserDailyTrackerRepository>();
}