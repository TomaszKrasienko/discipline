using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.infrastructure.DAL;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.CacheDecorators;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Repositories;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using MongoDB.Driver;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(
        this IServiceCollection services,
        string moduleName)
        => services
            .AddScoped<DailyTrackersMongoContext>(sp =>
            {
                var mongoClient = sp.GetRequiredService<IMongoClient>();
                var mongoCollectionNameConvention = sp.GetRequiredService<IMongoCollectionNameConvention>();
                return new DailyTrackersMongoContext(
                    mongoClient,
                    mongoCollectionNameConvention,
                    moduleName);
            })
            .AddScoped<IReadDailyTrackerRepository, MongoDailyTrackerRepository>()
            .AddScoped<IWriteDailyTrackerRepository, MongoDailyTrackerRepository>()
            .AddDecorators();

    private static IServiceCollection AddDecorators(this IServiceCollection services)
    {
        services.TryDecorate(typeof(IWriteDailyTrackerRepository), typeof(CacheDailyTrackerRepositoryDecorator));

        return services;
    }
}