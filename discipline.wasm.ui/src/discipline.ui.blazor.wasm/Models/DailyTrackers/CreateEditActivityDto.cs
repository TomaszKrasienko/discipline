namespace discipline.ui.blazor.wasm.Models.DailyTrackers;

public sealed class CreateEditActivityDto
{
    public string? Title { get; set; }
    public string? Note { get; set; }
    public DateOnly Day { get; set; }
}