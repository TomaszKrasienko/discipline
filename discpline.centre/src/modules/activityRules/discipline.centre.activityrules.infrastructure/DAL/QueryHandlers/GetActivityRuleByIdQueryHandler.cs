using discipline.centre.activityrules.application.ActivityRules.DTOs.Responses;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Responses.ActivityRules;
using discipline.centre.activityrules.application.ActivityRules.Queries;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.shared.abstractions.CQRS.Queries;
using MongoDB.Driver;

namespace discipline.centre.activityrules.infrastructure.DAL.QueryHandlers;

internal sealed class GetActivityRuleByIdQueryHandler(
    ActivityRulesMongoContext context) : IQueryHandler<GetActivityRuleByIdQuery, ActivityRuleResponseDto?>
{
    public async Task<ActivityRuleResponseDto?> HandleAsync(GetActivityRuleByIdQuery query, CancellationToken cancellationToken = default)
        => (await context.GetCollection<ActivityRuleDocument>()
            .Find(x 
                => x.Id == query.ActivityRuleId.ToString()
                && x.UserId == query.AccountId.ToString())
            .SingleOrDefaultAsync(cancellationToken))?.AsResponseDto();
}