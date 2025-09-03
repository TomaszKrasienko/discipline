using discipline.libs.mongo_db.Abstractions;

namespace discipline.daily_trackers.infrastructure.DAL.DailyTrackers.Documents;

public sealed record UserDailyTrackerDocument : IDocument
{
    public required string Id { get; init; }
    public required string AccountId { get; init; }
    public DateOnly Day { get; set; }
    public string? Next { get; set; }
    public string? Prior { get; set; }
    public required List<ActivityDocument> Activities { get; init; }
}