namespace discipline.activity_scheduler.add_planned_tasks.DTOs;

internal sealed record ActiveModesDto
{
    public required List<string> Modes { get; init; }
    public int Day { get; init; }
}