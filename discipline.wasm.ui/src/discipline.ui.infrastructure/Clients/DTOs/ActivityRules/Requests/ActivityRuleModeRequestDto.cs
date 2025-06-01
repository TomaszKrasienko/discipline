namespace discipline.ui.infrastructure.Clients.DTOs.ActivityRules.Requests;

internal sealed record ActivityRuleModeRequestDto(string Mode, HashSet<int>? Days);