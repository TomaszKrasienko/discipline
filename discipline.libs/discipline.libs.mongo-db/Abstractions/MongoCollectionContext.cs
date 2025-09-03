using MongoDB.Driver;

namespace discipline.libs.mongo_db.Abstractions;

public abstract class MongoCollectionContext(
    IMongoClient mongoClient,
    IMongoCollectionNameConvention mongoCollectionNameConvention,
    string databaseName) : IMongoCollectionContext
{
    public IMongoCollection<T> GetCollection<T>() where T : IDocument
    {
        var mongoDatabase = mongoClient.GetDatabase(databaseName);
        var collectionName = mongoCollectionNameConvention.GetCollectionName<T>();
        return mongoDatabase.GetCollection<T>(collectionName);
    }
}