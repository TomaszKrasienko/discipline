namespace discipline.hangfire.add_activity_rules.DTOs;

internal sealed record SelectedModeDto
{
    public required string Mode { get; init; }
    public IReadOnlyList<int>? SelectedDays { get; init; }
}