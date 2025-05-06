namespace discipline.hangfire.add_planned_tasks.DTOs;

public sealed record ActiveModesDto
{
    public required List<string> Modes { get; init; }
    public int Day { get; init; }
}