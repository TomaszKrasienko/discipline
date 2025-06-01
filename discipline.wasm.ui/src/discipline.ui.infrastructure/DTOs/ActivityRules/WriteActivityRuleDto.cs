namespace discipline.ui.infrastructure.DTOs.ActivityRules;

public sealed record WriteActivityRuleDto
{
    public required string Title { get; init; }
    public string? Note { get; init; }
    public required string Mode { get; init; }
    public HashSet<int>? Days { get; init; }
}