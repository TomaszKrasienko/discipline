namespace discipline.daily_trackers.application.UserDailyTrackers.DTOs.Responses;

public sealed record ActivityResponseDto(
    ActivityDetailsResponseDto Details,
    bool IsChecked,
    string ParentActivityRuleId,
    IReadOnlyCollection<StageResponseDto> Stages);