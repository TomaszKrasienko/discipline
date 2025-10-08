namespace discipline.daily_trackers.application.UserDailyTrackers.DTOs.Responses;

public sealed record ActivityDetailsResponseDto(
    string Title,
    string? Note);