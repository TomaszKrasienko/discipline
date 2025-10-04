namespace discipline.daily_trackers.application.UserDailyTrackers.DTOs.Requests;

public sealed record ActivityDetailsRequestDto(
    string Title,
    string? Note);