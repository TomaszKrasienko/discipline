namespace discipline.ui.infrastructure.Clients.DTOs.ActivityRules.Responses;

internal sealed record ActivityRuleResponseDto(string ActivityRuleId, 
    ActivityRuleDetailsResponseDto Details,
    ActivityRuleSelectedModeResponseDto Mode, 
    List<ActivityRuleStageResponseDto> Stages);