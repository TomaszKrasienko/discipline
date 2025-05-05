namespace discipline.ui.communication.http.DailyTrackers.Responses;

public sealed record DailyTrackerResponseDto(string DailyTrackerId, DateOnly Day, IReadOnlyCollection<ActivityResponseDto> Activities);