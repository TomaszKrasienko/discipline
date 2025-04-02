namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Responses;

public sealed record ActivityRuleResponseDto(string ActivityRuleId, 
    string Title, 
    string? Note,
    string Mode, 
    IReadOnlyCollection<int>? SelectedDays, 
    List<StageResponseDto> Stages);