using discipline.centre.shared.infrastructure.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace discipline.centre.activityrules.infrastructure.DAL.Documents;

internal sealed record ActivityRuleDocument : IDocument
{
    [BsonId]
    public required string Id { get; init; }
    public required string AccountId { get; init; }
    public required ActivityRuleDetailsDocument Details { get; init; }
    public required ActivityRuleSelectedModeDocument SelectedMode { get; init; }
    public required IEnumerable<StageDocument> Stages { get; init; }
}