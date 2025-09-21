using discipline.libs.mongo_db.Abstractions;

namespace discipline.libs.mongo_db;

internal sealed class MongoCollectionNameConvention : IMongoCollectionNameConvention
{
    public string GetCollectionName<T>() where T : IDocument
        => $"{typeof(T).Name}s";
}