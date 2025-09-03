using discipline.libs.mongo_db.Abstractions;

namespace discipline.daily_trackers.infrastructure.DAL.DailyTrackers.Documents;

public sealed record StageDocument : IDocument
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public int Index { get; init; }
    public bool IsChecked { get; init; }
}