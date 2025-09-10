using discipline.hangfire.activity_rule_modification.Events.External;
using discipline.hangfire.activity_rule_modification.Strategies.Abstractions;
using discipline.hangfire.domain.ActivityRules;
using discipline.hangfire.shared.abstractions.DAL;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.Extensions.Logging;

namespace discipline.hangfire.activity_rule_modification.Strategies;

internal sealed class ActivityRuleChangedStrategy(
    ILogger<ActivityRuleChangedStrategy> logger,
    IWriteRepository<ActivityRule, ActivityRuleId> activityRuleRepository) : IActivityRuleHandlingStrategy
{
    public async Task HandleAsync(ActivityRuleModified @event, CancellationToken cancellationToken)
    {
        if (@event.Title is null ||
            @event.Mode is null)
        {
            throw new ArgumentException("Title and Mode cannot be null");
        }
        
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

        activityRule.UpdateMode(
            @event.Title,
            @event.Mode,
            @event.Days);
        
        await activityRuleRepository.SaveChangesAsync(cancellationToken);
    }

    public bool CanBeApplied(string messageType)
        => messageType.EndsWith("Changed");
}