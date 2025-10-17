namespace discipline.daily_trackers.application.UserDailyTrackers.DTOs.Responses;

public sealed record ActivityResponseDto(
    string Id,
    ActivityDetailsResponseDto Details,
    bool IsChecked,
    string ParentActivityRuleId,
    IReadOnlyCollection<StageResponseDto> Stages);