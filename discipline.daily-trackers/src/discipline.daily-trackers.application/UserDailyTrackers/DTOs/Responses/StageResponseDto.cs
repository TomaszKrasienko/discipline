namespace discipline.daily_trackers.application.UserDailyTrackers.DTOs.Responses;

public sealed record StageResponseDto(
    string Title,
    int Index,
    bool IsChecked);