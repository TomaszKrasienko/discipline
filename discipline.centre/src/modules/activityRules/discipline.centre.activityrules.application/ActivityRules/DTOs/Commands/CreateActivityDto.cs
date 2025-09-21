namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Commands;

public sealed record CreateActivityDto(
    string ActivityRuleId,
    CreateActivityDetailsDto Details,
    IReadOnlyCollection<CreateActivityStageDto> Stages);