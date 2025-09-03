using discipline.libs.mongo_db.Abstractions;

namespace discipline.daily_trackers.infrastructure.DAL.DailyTrackers.Documents;

public sealed record ActivityDocument : IDocument
{
    public required string Id { get; init; }
    public required ActivityDetailsDocument Details { get; init; }
    public bool IsChecked { get; init; }
    public string? ParentActivityRuleId { get; init; }
    public required List<StageDocument> Stages { get; init; }
}