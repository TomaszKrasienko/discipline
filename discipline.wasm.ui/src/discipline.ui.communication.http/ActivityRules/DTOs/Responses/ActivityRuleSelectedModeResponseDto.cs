namespace discipline.ui.communication.http.ActivityRules.DTOs.Responses;

public sealed record ActivityRuleSelectedModeResponseDto(string Mode, HashSet<int>? Days);