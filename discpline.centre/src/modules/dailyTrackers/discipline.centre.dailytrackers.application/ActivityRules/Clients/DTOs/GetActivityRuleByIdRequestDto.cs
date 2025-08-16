using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.ActivityRules.Clients.DTOs;

public sealed record GetActivityRuleByIdRequestDto(ActivityRuleId ActivityRuleId, AccountId AccountId);