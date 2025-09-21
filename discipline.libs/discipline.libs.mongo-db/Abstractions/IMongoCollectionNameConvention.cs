namespace discipline.libs.mongo_db.Abstractions;

public interface IMongoCollectionNameConvention
{
    string GetCollectionName<T>() where T : IDocument;
}