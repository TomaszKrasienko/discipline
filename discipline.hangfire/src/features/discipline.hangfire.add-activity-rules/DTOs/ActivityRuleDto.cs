namespace discipline.hangfire.add_activity_rules.DTOs;

internal sealed record ActivityRuleDto
{
    public required Ulid ActivityRuleId { get; init; }
    public required string Mode { get; init; }
    public IReadOnlyList<int>? SelectedDays { get; init; }
}