namespace discipline.centre.activityrules.application.ActivityRules.DTOs.Responses.ActivityRules;

public sealed record SelectedModeResponseDto(string Mode, IReadOnlyCollection<int>? Days);