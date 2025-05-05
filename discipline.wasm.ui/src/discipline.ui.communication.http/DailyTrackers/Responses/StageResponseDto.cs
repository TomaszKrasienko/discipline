namespace discipline.ui.communication.http.DailyTrackers.Responses;

public sealed record StageResponseDto(string StageId, string Title, int Index, bool IsChecked);