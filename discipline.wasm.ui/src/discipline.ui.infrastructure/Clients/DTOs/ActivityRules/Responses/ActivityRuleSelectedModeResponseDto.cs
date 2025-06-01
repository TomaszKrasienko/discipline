namespace discipline.ui.infrastructure.Clients.DTOs.ActivityRules.Responses;

internal sealed record ActivityRuleSelectedModeResponseDto(string Mode, HashSet<int>? Days);