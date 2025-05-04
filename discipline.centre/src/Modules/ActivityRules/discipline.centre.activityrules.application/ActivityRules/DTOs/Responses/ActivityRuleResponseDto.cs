namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Responses;

public sealed record ActivityRuleResponseDto(string ActivityRuleId, 
    DetailsResponseDto Details,
    SelectedModeResponseDto Mode, 
    List<StageResponseDto> Stages);