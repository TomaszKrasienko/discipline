using discipline.centre.activityrules.application.ActivityRules.DTOs.Responses.Stages;

namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Responses.ActivityRules;

public sealed record ActivityRuleResponseDto(string ActivityRuleId, 
    DetailsResponseDto Details,
    SelectedModeResponseDto Mode, 
    List<StageResponseDto> Stages);