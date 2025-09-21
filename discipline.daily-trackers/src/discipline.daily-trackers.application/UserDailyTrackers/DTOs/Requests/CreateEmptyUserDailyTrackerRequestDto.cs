namespace discipline.daily_trackers.application.UserDailyTrackers.DTOs.Requests;

public sealed record CreateEmptyUserDailyTrackerRequestDto(
    string DailyTrackerId,
    string AccountId,
    DateOnly Day);