namespace discipline.daily_trackers.application.UserDailyTrackers.DTOs.Requests;

public sealed record CreateActivityRequestDto(
    DateOnly Day,
    ActivityDetailsRequestDto ActivityDetails);