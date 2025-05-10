namespace discipline.hangfire.add_activity_rules.DTOs;

internal sealed record ActivityRuleDto
{
    public required Ulid ActivityRuleId { get; init; }
    public required DetailsDto Details { get; init; }
    public required SelectedModeDto Mode { get; init; }
}