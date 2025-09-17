namespace discipline.daily_trackers.application.UserDailyTrackers.DTOs.ExternalCommands;

public sealed record CreateActivityDto(
    string ActivityRuleId,
    CreateActivityDetailsDto Details,
    IReadOnlyCollection<CreateActivityStageDto> Stages);