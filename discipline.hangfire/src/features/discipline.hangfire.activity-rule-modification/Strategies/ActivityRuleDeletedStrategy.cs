using discipline.hangfire.activity_rule_modification.Events.External;
using discipline.hangfire.activity_rule_modification.Strategies.Abstractions;
using discipline.hangfire.domain.ActivityRules;
using discipline.hangfire.shared.abstractions.DAL;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.activity_rule_modification.Strategies;

internal sealed class ActivityRuleDeletedStrategy(
    ILogger<ActivityRuleDeletedStrategy> logger,
    IWriteRepository<ActivityRule, ActivityRuleId> activityRuleRepository) : IActivityRuleHandlingStrategy
{
    public async Task HandleAsync(ActivityRuleModified @event, CancellationToken cancellationToken)
    {
        var stronglyActivityRuleId = ActivityRuleId.Parse(@event.ActivityRuleId);
        var stronglyUserId = AccountId.Parse(@event.UserId);
        
        var activityRule = await activityRuleRepository
            .SingleOrDefaultAsync(x => 
                x.ActivityRuleId == stronglyActivityRuleId && 
                x.AccountId == stronglyUserId, cancellationToken);

        if (activityRule is null)
        {
            logger.LogInformation($"Activity rule {stronglyActivityRuleId} not found");
            return;
        }
        
        await activityRuleRepository.DeleteAsync(activityRule, cancellationToken);
    }

    public bool CanBeApplied(string messageType)
        => messageType.EndsWith("Deleted");
}