namespace discipline.ui.communication.http.ActivityRules.DTOs.Responses;

public sealed record ActivityRuleResponseDto(string ActivityRuleId, 
    ActivityRuleDetailsResponseDto Details,
    ActivityRuleSelectedModeResponseDto Mode, 
    List<ActivityRuleStageResponseDto> Stages);