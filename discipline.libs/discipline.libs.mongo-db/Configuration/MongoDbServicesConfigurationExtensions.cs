using discipline.libs.configuration;
using discipline.libs.mongo_db.Abstractions;
using discipline.libs.mongo_db.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace discipline.libs.mongo_db.Configuration;

public static class MongoDbServicesConfigurationExtensions
{
    public static IServiceCollection AddMongoDb(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .ValidateAndAddOptions(configuration)
            .AddConnection()
            .AddConventions();
    
    private static IServiceCollection ValidateAndAddOptions(
        this IServiceCollection services,
        IConfiguration configuration)
        => services.ValidateAndBind<MongoDbOptions, MongoDbOptionsValidator>(configuration);
    
    private static IServiceCollection AddConnection(this IServiceCollection services)
    {
        var mongoOptions = services.GetOptions<MongoDbOptions>();
        services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoOptions.ConnectionString));
        return services;
    }

    private static IServiceCollection AddConventions(this IServiceCollection services)
        => services
            .AddSingleton<IMongoCollectionNameConvention, MongoCollectionNameConvention>();
}