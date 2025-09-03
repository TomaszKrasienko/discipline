using discipline.libs.mongo_db.Abstractions;
using MongoDB.Driver;

namespace discipline.daily_trackers.infrastructure.DAL;

internal sealed class DailyTrackerMongoContext(
    IMongoClient mongoClient,
    IMongoCollectionNameConvention mongoCollectionNameConvention,
    string appName) : MongoCollectionContext(mongoClient, mongoCollectionNameConvention, appName)
{
    
}