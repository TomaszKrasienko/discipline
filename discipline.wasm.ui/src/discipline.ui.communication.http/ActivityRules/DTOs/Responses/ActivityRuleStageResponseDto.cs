namespace discipline.ui.communication.http.ActivityRules.DTOs.Responses;

public sealed record ActivityRuleStageResponseDto(string StageId, 
    string Title, 
    int Index);