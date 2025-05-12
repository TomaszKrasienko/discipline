namespace discipline.hangfire.activity_rules.DTOs;

internal sealed record SelectedModeDto
{
    public required string Mode { get; init; }
    public IReadOnlyList<int>? Days { get; init; }
}