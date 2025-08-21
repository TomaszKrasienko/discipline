using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using MongoDB.Driver;

namespace discipline.centre.calendar.infrastructure.DAL;

internal sealed class CalendarMongoContext(
    IMongoClient mongoClient, 
    IMongoCollectionNameConvention mongoCollectionNameConvention,
    string moduleName) 
    : MongoCollectionContext(mongoClient, mongoCollectionNameConvention, moduleName);