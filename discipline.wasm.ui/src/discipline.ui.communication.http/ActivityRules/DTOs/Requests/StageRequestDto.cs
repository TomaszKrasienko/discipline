namespace discipline.ui.communication.http.ActivityRules.DTOs.Requests;

public sealed record StageRequestDto
{
    public required string Title { get; init; }
    public int Index { get; init; }
}