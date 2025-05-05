namespace discipline.ui.communication.http.DailyTrackers.Responses;

public sealed record ActivityResponseDto(string ActivityId, ActivityDetailsSpecificationResponseDto Details, 
    bool IsChecked, string ParentActivityRuleId, IReadOnlyCollection<StageResponseDto>? Stages);