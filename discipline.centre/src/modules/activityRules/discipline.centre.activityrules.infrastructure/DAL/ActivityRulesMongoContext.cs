using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using MongoDB.Driver;

namespace discipline.centre.activity_rules.infrastructure.DAL;

internal sealed class ActivityRulesMongoContext(
    IMongoClient mongoClient, 
    IMongoCollectionNameConvention mongoCollectionNameConvention,
    string moduleName) 
    : MongoCollectionContext(mongoClient, mongoCollectionNameConvention, moduleName)
{
    
}