using MongoDB.Driver;

namespace discipline.libs.mongo_db.Abstractions;

public interface IMongoCollectionContext
{
    IMongoCollection<T> GetCollection<T>() where T : IDocument;
}