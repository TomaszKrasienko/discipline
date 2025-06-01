namespace discipline.ui.infrastructure.DTOs.ActivityRules;

public sealed record ActivityRuleDto(string ActivityRuleId, 
    ActivityRuleDetailsDto Details,
    ActivityRuleSelectedModeDto Mode, 
    List<ActivityRuleStageDto> Stages);