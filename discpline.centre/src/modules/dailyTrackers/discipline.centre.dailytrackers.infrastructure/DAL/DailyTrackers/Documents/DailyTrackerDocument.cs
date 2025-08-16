using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;

public sealed record DailyTrackerDocument : IDocument
{
    [BsonId]
    public required string DailyTrackerId { get; init; }
    public DateOnly Day { get; init; }
    public required string AccountId { get; init; }
    public required IEnumerable<ActivityDocument> Activities { get; init; }
}