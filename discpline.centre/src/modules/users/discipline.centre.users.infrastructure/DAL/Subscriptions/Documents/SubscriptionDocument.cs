using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;

internal sealed class SubscriptionDocument : IDocument
{
    [BsonId]
    public required string Id { get; set; }
    public required string Type { get; set; }
    public required List<PriceDocument> Prices { get; set; }
}