namespace discipline.ui.infrastructure.Clients.DTOs.ActivityRules.Responses;

internal sealed record ActivityRuleStageResponseDto(string StageId, 
    string Title, 
    int Index);