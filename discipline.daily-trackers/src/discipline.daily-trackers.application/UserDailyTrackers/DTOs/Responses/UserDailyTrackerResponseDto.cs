namespace discipline.daily_trackers.application.UserDailyTrackers.DTOs.Responses;

public sealed record UserDailyTrackerResponseDto(
    string Id,
    string AccountId,
    DateOnly Day,
    string? Next,
    string? Prior,
    IReadOnlyCollection<ActivityResponseDto> Activities);