using discipline.centre.activity_rules.infrastructure.DAL;
using discipline.centre.activity_rules.infrastructure.DAL.Documents;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Responses.ActivityRules;
using discipline.centre.activityrules.application.ActivityRules.Queries;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.libs.cqrs.abstractions.Queries;
using MongoDB.Driver;

namespace discipline.centre.activity_rules.infrastructure.QueryHandlers;

internal sealed class GetActivityRuleByIdQueryHandler(
    ActivityRulesMongoContext context) : IQueryHandler<GetActivityRuleByIdQuery, ActivityRuleResponseDto?>
{
    public async Task<ActivityRuleResponseDto?> HandleAsync(GetActivityRuleByIdQuery query, CancellationToken cancellationToken = default)
        => (await context.GetCollection<ActivityRuleDocument>()
            .Find(x 
                => x.Id == query.ActivityRuleId.ToString()
                && x.AccountId == query.AccountId.ToString())
            .SingleOrDefaultAsync(cancellationToken))?.AsResponseDto();
}