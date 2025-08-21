using discipline.centre.shared.abstractions.UnitOfWork;
using discipline.centre.shared.infrastructure.DAL;
using discipline.centre.shared.infrastructure.DAL.Collections;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using discipline.centre.shared.infrastructure.DAL.Configuration;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DalSharedServicesConfigurationExtensions
{
    internal static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
        => services
            .ValidateAndAddOptions(configuration)
            .AddConnection()
            .AddCollections()
            .AddUnitOfWork();

    private static IServiceCollection ValidateAndAddOptions(this IServiceCollection services,
        IConfiguration configuration)
        => services.ValidateAndBind<MongoDbOptions, MongoDbOptionsValidator>(configuration);
    
    private static IServiceCollection AddConnection(this IServiceCollection services)
    {
        var mongoOptions = services.GetOptions<MongoDbOptions>();
        services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoOptions.ConnectionString));
        return services;
    }

    private static IServiceCollection AddCollections(this IServiceCollection services)
        => services
            .AddSingleton<IMongoCollectionNameConvention, MongoCollectionNameConvention>();

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        => services
            .AddScoped<IUnitOfWork, MongoUnitOfWork>();

    public static IServiceCollection AddRepositories<TReadRepository, TReadWriteRepository, TRepository>(
        this IServiceCollection services) 
        where TRepository : class, TReadRepository, TReadWriteRepository 
        where TReadRepository : class
        where TReadWriteRepository : class 
        => services
            .AddScoped<TReadRepository, TRepository>()
            .AddScoped<TReadWriteRepository, TRepository>();
    
}