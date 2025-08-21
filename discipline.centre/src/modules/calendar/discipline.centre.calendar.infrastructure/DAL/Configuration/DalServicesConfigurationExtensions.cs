using discipline.centre.calendar.domain.Repositories;
using discipline.centre.calendar.infrastructure.DAL;
using discipline.centre.calendar.infrastructure.DAL.Repositories;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using MongoDB.Driver;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

internal static class DalServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services, string moduleName)
        => services
            .AddScoped<CalendarMongoContext>(sp =>
            {
                var mongoClient = sp.GetRequiredService<IMongoClient>();
                var mongoCollectionNameConvention = sp.GetRequiredService<IMongoCollectionNameConvention>();
                return new CalendarMongoContext(
                    mongoClient,
                    mongoCollectionNameConvention,
                    moduleName);
            })
            .AddScoped<IReadUserCalendarRepository, MongoUserCalendarDayRepository>()
            .AddScoped<IReadWriteUserCalendarRepository, MongoUserCalendarDayRepository>();
}