using discipline.libs.mongo_db.Abstractions;

namespace discipline.daily_trackers.infrastructure.DAL.DailyTrackers.Documents;

public sealed record ActivityDetailsDocument : IDocument
{
    public required string Title { get; init; }
    public string? Note { get; init; }
}