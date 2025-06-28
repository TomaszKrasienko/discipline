using discipline.centre.activityrules.application.ActivityRules.DTOs.Responses;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Responses.ActivityRules;
using discipline.centre.shared.abstractions.CQRS.Queries;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.application.ActivityRules.Queries;

public sealed record GetActivityRulesQuery(UserId UserId) : IQuery<IReadOnlyCollection<ActivityRuleResponseDto>>;