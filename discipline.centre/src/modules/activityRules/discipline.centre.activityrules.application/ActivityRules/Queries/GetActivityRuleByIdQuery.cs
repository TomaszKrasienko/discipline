using discipline.centre.activityrules.application.ActivityRules.DTOs.Responses.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.abstractions.Queries;

namespace discipline.centre.activityrules.application.ActivityRules.Queries;

public sealed record GetActivityRuleByIdQuery(AccountId AccountId, ActivityRuleId ActivityRuleId) : IQuery<ActivityRuleResponseDto?>;